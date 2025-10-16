#pragma warning disable WFO1000 // Missing code serialization configuration for property content
namespace Tripous.Forms
{
    public class MarkdownWriterWebView : WebView2
    {
        const string DefaultCssText = @"
body {
    font-family: 'Segoe UI', sans-serif;
    background-color: #f8f9fa;
    color: #222;
    margin: 20px;
}
h1, h2, h3 {
    color: #005bbb;
}
code {
    background-color: #eee;
    padding: 2px 4px;
    border-radius: 3px;
    font-family: Consolas, monospace;
}
pre {
    background-color: #f0f0f0;
    padding: 8px;
    border-radius: 5px;
}
img { max-width: 100%; height: auto; }

 
/* GitHub-like styling for HTML tables in Markdown previews */
table {
  border-collapse: collapse;
  border-spacing: 0;
  display: block;
  width: max-content;
  max-width: 100%;
  overflow: auto;
  margin: 16px 0;
}

/* Borders and cell padding */
table th,
table td {
  border: 1px solid #d0d7de;
  padding: 6px 13px;
  vertical-align: top;
}

/* Header look */
table thead th {
  background-color: #f6f8fa;
  font-weight: 600;
}

/* Zebra striping */
table tbody tr:nth-child(2n) {
  background-color: #f6f8fa;
}

/* Compact subtext within cells */
table small {
  color: #57606a;
}

/* Keep code readable in cells */
table code {
  background: none;
  border: 0;
  padding: 0;
  font-family: Consolas, Menlo, Monaco, ""Liberation Mono"", monospace;
  font-size: 85%;
  color: #24292f;
}

/* Images should not overflow cells */
table img {
  max-width: 100%;
  height: auto;
}

/* Caption styling */
table caption {
  caption-side: bottom;
  text-align: left;
  color: #57606a;
  font-size: 90%;
  padding-top: 6px;
}

/* Alignment support from HTML align attribute */
table th[align=""center""],
table td[align=""center""] { text-align: center; }

table th[align=""right""],
table td[align=""right""] { text-align: right; }

table th[align=""left""],
table td[align=""left""] { text-align: left; }

/* Prevent overly tall rows from collapsing line-height */
table th,
table td {
  line-height: 1.4;
}

/* Hover highlight, subtle */
table tbody tr:hover {
  background-color: #eef2f6;
}

/* Sticky header (optional, comment out if undesired) */
table thead th {
  position: sticky;
  top: 0;
  z-index: 1;
}

/* Responsive tweak: avoid horizontal jitter from scrollbars */
table::-webkit-scrollbar {
  height: 10px;
}
table::-webkit-scrollbar-thumb {
  background-color: #c9d1d9;
  border-radius: 8px;
  border: 2px solid #f6f8fa;
}
table::-webkit-scrollbar-track {
  background: #f6f8fa;
}
 

blockquote {
  margin: 1.2em 2em;
  padding: 1em 1.5em;
  background: #f2f6fb;
  border-left: 5px solid #3a82c2;
  border-radius: 4px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  color: #222;
  line-height: 1.2;
}
blockquote p {
  margin: 0.3em 0;
}

";

        // ● Virtual host
        const string AssetsHost = "assets"; // => https://assets/

        // ● Render / pipeline
        MarkdownPipeline Pipeline;
        System.Windows.Forms.Timer Timer;
        CancellationTokenSource CTS;

        /// <summary>
        /// Project root folder (e.g., C:\Projects\Project1).
        /// MUST contain subfolders: Components, Images (sibling).
        /// </summary>
        string fAssetsRootPath;
        /// <summary>
        /// Markdown folder name (e.g., C:\Projects\Project1\Docs).
        /// </summary>
        string fMarkdownFolderName;

        string fMarkdownText;
 
        // ● State
        bool IsAssetsMapped;
        string PendingHtmlText;
 
        // ● Scroll state
        double LastScrollY = 0;

        // ● private
        /// <summary>
        /// Ensures WebView2 is initialized.
        /// </summary>
        async Task EnsureInitializedAsync()
        {
            if (IsInitialized) 
                return;

            try
            {
                if (!string.IsNullOrWhiteSpace(UserDataFolder))
                {
                    var env = await CoreWebView2Environment.CreateAsync(userDataFolder: UserDataFolder);
                    await EnsureCoreWebView2Async(env);
                }
                else
                {
                    await EnsureCoreWebView2Async();
                }

                IsInitialized = true;

                EnsureAssetsMapping();      // Map project root -> https://assets/

                if (!string.IsNullOrEmpty(PendingHtmlText))
                {
                    await RenderHtmlTextAsync(PendingHtmlText);
                    PendingHtmlText = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WebView2 init failed: " + ex);
            }
        }
        /// <summary>
        /// Maps the project root folder to a virtual host.
        /// </summary>
        void EnsureAssetsMapping()
        {
            if (IsAssetsMapped) return;
            if (CoreWebView2 == null) return;
            if (string.IsNullOrWhiteSpace(AssetsRootPath)) return;
            if (!Directory.Exists(AssetsRootPath)) return;

            CoreWebView2.SetVirtualHostNameToFolderMapping(
                AssetsHost,
                AssetsRootPath, // e.g., C:\Projects\Project1
                CoreWebView2HostResourceAccessKind.Allow
            );
            IsAssetsMapped = true;
        }
        /// <summary>
        /// Event handler for WebView2 initialization completion.
        /// </summary>
        void OnCoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            IsInitialized = e.IsSuccess;
            if (!e.IsSuccess)
            {
                System.Diagnostics.Debug.WriteLine("WebView2 init error: " + e.InitializationException?.Message);
                return;
            }

            EnsureAssetsMapping();

            if (!string.IsNullOrEmpty(PendingHtmlText))
            {
                try 
                {
                    _ = RenderHtmlTextAsync(PendingHtmlText); 
                } 
                catch 
                { 
                    /* ignore */ 
                }

                PendingHtmlText = null;
            }
        }

        // ● html
        /// <summary>
        /// Wraps HTML to a complete doc.
        /// </summary>
        string WrapHtml(string HtmlText)
        {
            string CssText = !string.IsNullOrWhiteSpace(ExternalCssText) ? ExternalCssText : DefaultCssText;

            string BaseRef = $"https://{AssetsHost}/";

            if (!string.IsNullOrWhiteSpace(MarkdownFolderName))
            {
                string FolderName = Path.GetDirectoryName(AssetsRootPath);
                if (string.Compare(MarkdownFolderName, FolderName, StringComparison.InvariantCultureIgnoreCase) != 0) 
                    BaseRef = $"https://{AssetsHost}/{MarkdownFolderName}/";
            } 

            return $@"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8"">
<base href=""{BaseRef}"">
<style>{CssText}</style>
</head>
<body>
{HtmlText}
</body>
</html>";
        }
        /// <summary>
        /// Fire-and-forget HTML set. Safe to call before initialization.
        /// </summary>
        void SetHtmlText(string HtmlText)
        {
            if (!IsInitialized)
            {
                PendingHtmlText = HtmlText ?? "";
                return;
            }
            _ = RenderHtmlTextAsync(HtmlText ?? string.Empty);
        }
        /// <summary>
        /// Called when HTML changed.
        /// <para>Renders the HTML text to the view.</para>
        /// </summary>
        async Task RenderHtmlTextAsync(string HtmlText)
        {
            LastScrollY = await GetScrollYAsync();

            string doc = WrapHtml(HtmlText ?? string.Empty);

            if (CoreWebView2 == null)
            {
                NavigateToString(doc);
                return;
            }

            void Handler(object s, CoreWebView2NavigationCompletedEventArgs e)
            {
                CoreWebView2.NavigationCompleted -= Handler;
                _ = RestoreScrollAsync(LastScrollY);
            }

            CoreWebView2.NavigationCompleted += Handler;
            NavigateToString(doc);
        }

        // ● markdown
        /// <summary>
        /// Called when Markdown changed.
        /// </summary>
        void MarkdownChanged()
        {
            if (Timer == null)
            {
                Timer = new System.Windows.Forms.Timer { Interval = Math.Max(50, DebounceMsecs) };
                Timer.Tick += async (_, __) =>
                {
                    Timer.Stop();
                    await RenderMarkdownAsync();
                };
                Timer.Start();
            }
            else
            {
                Timer.Stop();
                Timer.Interval = Math.Max(50, DebounceMsecs);
                Timer.Start();
            }
        }
        /// <summary>
        /// Renders Markdown to HTML.
        /// </summary>
        async Task RenderMarkdownAsync()
        { 
            CTS?.Cancel();
            CTS = new CancellationTokenSource();
            var Token = CTS.Token;
 
            string HtmlText;
            try
            {
                HtmlText = await Task.Run(() =>
                {
                    Token.ThrowIfCancellationRequested();
                    return Markdig.Markdown.ToHtml(MarkdownText, Pipeline);
                }, Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) 
            { 
                return; 
            }
            catch (Exception ex)
            {
                HtmlText = $"<pre>Markdown render error:\n{System.Net.WebUtility.HtmlEncode(ex.Message)}</pre>";
            }

            if (Token.IsCancellationRequested) 
                return;

            if (IsHandleCreated)
            {
                if (InvokeRequired) 
                    BeginInvoke(new Action(() => SetHtmlText(HtmlText)));
                else
                    SetHtmlText(HtmlText);
            }
        }


        /// <summary>
        /// Reads current scrollY from the page.
        /// </summary>
        async Task<double> GetScrollYAsync()
        {
            try
            {
                if (CoreWebView2 == null) return 0;
                string json = await CoreWebView2.ExecuteScriptAsync("(() => JSON.stringify(window.scrollY || 0))()");
                if (string.IsNullOrWhiteSpace(json)) return 0;
                json = json.Trim('"');
                if (double.TryParse(json, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var y))
                    return y;
            }
            catch { }
            return 0;
        }
        /// <summary>
        /// Restores scrollY after navigation/render.
        /// </summary>
        async Task RestoreScrollAsync(double y)
        {
            try
            {
                if (CoreWebView2 == null) return;
                string yStr = y.ToString(System.Globalization.CultureInfo.InvariantCulture);
                await CoreWebView2.ExecuteScriptAsync($@"
                    (function() {{
                        const y = {yStr};
                        window.scrollTo(0, y);
                        setTimeout(() => window.scrollTo(0, y), 50);
                        setTimeout(() => window.scrollTo(0, y), 200);
                    }})();
                ");
            }
            catch { }
        }

        // ● overrides
        /// <summary>
        /// Ensures WebView2 is initialized.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode)
                _ = EnsureInitializedAsync();
        }
        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Timer?.Dispose();
                CTS?.Cancel();
                CTS?.Dispose();
            }
            base.Dispose(disposing);
        }


        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public MarkdownWriterWebView()
        {
            Dock = DockStyle.Fill;

            Pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build(); 

            CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;
        }

        // ● public
        /// <summary>
        /// Loads Markdown from file and adjusts assets root path to the file directory.
        /// </summary>
        public void LoadFromFile(string MarkdownFilePath)
        {
            if (File.Exists(MarkdownFilePath))
            {
                string Text = File.ReadAllText(MarkdownFilePath);
                AssetsRootPath = Path.GetDirectoryName(MarkdownFilePath);
                MarkdownText = Text;
            }
        }

        // ● properties
        /// <summary>
        /// True when CoreWebView2 initialization is completed. 
        /// </summary>
        public bool IsInitialized { get; private set; }
        /// <summary>
        /// Environment data folder.
        /// <para>SEE <see cref="CoreWebView2Environment.CreateAsync"/>()</para>
        /// </summary>
        public string UserDataFolder { get; set; }
        /// <summary>
        /// External CSS text
        /// </summary>
        public string ExternalCssText { get; set; }
        /// <summary>
        /// Markdown text. Setting this to a value triggers the whole rendering process.
        /// </summary>
        public string MarkdownText
        {
            get => fMarkdownText;
            set
            {
                if (fMarkdownText != value)
                {
                    fMarkdownText = value;
                    MarkdownChanged();
                }
            }
        }
        /// <summary>
        /// Debounce time in milliseconds of how to delay rendering when MarkdownText is changed.
        /// </summary>
        public int DebounceMsecs { get; set; } = 400;
        /// <summary>
        /// A path to the assets folder.
        /// <para>It must be the container of markdown and image files or folders.</para>
        /// <para>AssetsRootPath is the root folder (e.g., C:\Projects\Project1).</para>
        /// <para>MUST contain subfolders: MarkdownFiles, Images (sibling).</para>
        /// </summary>
        public string AssetsRootPath
        {
            get => fAssetsRootPath;
            set
            {
                fAssetsRootPath = value;
                if (IsAssetsMapped)
                {
                    IsAssetsMapped = false;
                    CoreWebView2.ClearVirtualHostNameToFolderMapping(AssetsHost);
                    EnsureAssetsMapping();
                }              
            }
        }
        /// <summary>
        /// <strong>Optional</strong>. The name of the markdown folder.
        /// <para>MUST be a child folder of AssetsRootPath.</para>
        /// <para>MUST be the name of the folder containing markdown files.</para>
        /// </summary>
        public string MarkdownFolderName
        {
            get => fMarkdownFolderName;
            set
            {
                if (fMarkdownFolderName != value)
                {
                    fMarkdownFolderName = value;
                }
            }
        }

 


    }
}

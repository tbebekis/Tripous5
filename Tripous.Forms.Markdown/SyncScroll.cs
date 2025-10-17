namespace Tripous.Forms
{

    public interface ISyncScrollAdapter
    {
        Task InitializeAsync();                 // κάνε ό,τι χρειάζεται για να είναι έτοιμο
        bool IsReady { get; }                   // true όταν μπορεί να γίνει Get/Set

        Task<double> GetRangeAsync();
        Task<double> GetPositionAsync();
        Task SetPositionAsync(double pos);

        Task<double> GetRatioAsync();
        Task SetRatioAsync(double ratio);

        event EventHandler Scrolled;            // πυροδοτείται όταν έγινε scroll
    }


    public sealed class FctbSyncScrollAdapter : ISyncScrollAdapter, IDisposable
    {
        private readonly FastColoredTextBox _fctb;

        public FctbSyncScrollAdapter(FastColoredTextBox fctb)
        {
            if (fctb == null) throw new ArgumentNullException("fctb");
            _fctb = fctb;
            _fctb.Scroll += OnFctbScroll;
            // Προαιρετικά: _fctb.VScroll += (_, __) => Scrolled?.Invoke(this, EventArgs.Empty);
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public bool IsReady { get { return !_fctb.IsDisposed && _fctb.IsHandleCreated; } }

        public Task<double> GetRangeAsync()
        {
            int range = _fctb.VerticalScroll.Maximum - _fctb.VerticalScroll.LargeChange;
            if (range < 0) range = 0;
            return Task.FromResult((double)range);
        }

        public Task<double> GetPositionAsync()
        {
            return Task.FromResult((double)_fctb.VerticalScroll.Value);
        }

        public Task SetPositionAsync(double pos)
        {
            int v = (int)Math.Round(pos);
            if (v < _fctb.VerticalScroll.Minimum) v = _fctb.VerticalScroll.Minimum;
            if (v > _fctb.VerticalScroll.Maximum) v = _fctb.VerticalScroll.Maximum;
            _fctb.VerticalScroll.Value = v;
            _fctb.Invalidate();
            return Task.CompletedTask;
        }

        public async Task<double> GetRatioAsync()
        {
            double range = await GetRangeAsync().ConfigureAwait(false);
            if (range <= 0) return 0.0;
            double pos = await GetPositionAsync().ConfigureAwait(false);
            double r = pos / range;
            if (r < 0) r = 0; else if (r > 1) r = 1;
            return r;
        }

        public async Task SetRatioAsync(double ratio)
        {
            if (ratio < 0) ratio = 0; else if (ratio > 1) ratio = 1;
            double range = await GetRangeAsync().ConfigureAwait(false);
            double target = ratio * Math.Max(1.0, range);
            await SetPositionAsync(target).ConfigureAwait(false);
        }

        public event EventHandler Scrolled;

        private void OnFctbScroll(object sender, ScrollEventArgs e)
        {
            var h = Scrolled;
            if (h != null) h(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _fctb.Scroll -= OnFctbScroll;
        }
    }
 


    public sealed class WebViewSyncScrollAdapter : ISyncScrollAdapter, IDisposable
    {
        private readonly WebView2 _web;
        private bool _initialized;

        public WebViewSyncScrollAdapter(WebView2 webView)
        {
            if (webView == null) throw new ArgumentNullException("webView");
            _web = webView;
            _web.CoreWebView2InitializationCompleted += OnInitCompleted;
            _web.NavigationCompleted += OnNavigationCompleted;
        }

        public async Task InitializeAsync()
        {
            // Προσπάθησε να εξασφαλίσεις CoreWebView2
            if (!IsReady)
            {
                try { await _web.EnsureCoreWebView2Async(); } catch { }
            }
            if (IsReady)
            {
                await InjectDocumentCreatedScript();
                HookWebMessage();
            }
            _initialized = true;
        }

        public bool IsReady
        {
            get
            {
                if (_web.IsDisposed) return false;
                return _web.CoreWebView2 != null;
            }
        }

        public async Task<double> GetRangeAsync()
        {
            if (!IsReady) return 0.0;
            string js = "(function(){const el=document.scrollingElement||document.documentElement||document.body;return el.scrollHeight - el.clientHeight;})();";
            string s = await SafeExec(js);
            return ParseJsNumber(s);
        }

        public async Task<double> GetPositionAsync()
        {
            if (!IsReady) return 0.0;
            string js = "(function(){const el=document.scrollingElement||document.documentElement||document.body;return el.scrollTop;})();";
            string s = await SafeExec(js);
            return ParseJsNumber(s);
        }

        public async Task SetPositionAsync(double pos)
        {
            if (!IsReady) return;
            string js = "(function(){const el=document.scrollingElement||document.documentElement||document.body;el.scrollTop=" + pos.ToString(System.Globalization.CultureInfo.InvariantCulture) + ";})();";
            await SafeExec(js);
        }

        public async Task<double> GetRatioAsync()
        {
            if (!IsReady) return 0.0;
            string js = "(function(){const el=document.scrollingElement||document.documentElement||document.body;const max=Math.max(1, el.scrollHeight - el.clientHeight);return el.scrollTop/max;})();";
            string s = await SafeExec(js);
            double r = ParseJsNumber(s);
            if (r < 0) r = 0; else if (r > 1) r = 1;
            return r;
        }

        public async Task SetRatioAsync(double ratio)
        {
            if (!IsReady) return;
            if (ratio < 0) ratio = 0; else if (ratio > 1) ratio = 1;
            string js = "(function(){const el=document.scrollingElement||document.documentElement||document.body;const max=Math.max(1, el.scrollHeight - el.clientHeight);el.scrollTop=" + ratio.ToString(System.Globalization.CultureInfo.InvariantCulture) + "*max;})();";
            await SafeExec(js);
        }

        public event EventHandler Scrolled;

        private async void OnInitCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;
            await InjectDocumentCreatedScript();
            HookWebMessage();

            // DOMContentLoaded: προσθέτουμε scroll listener και για ήδη φορτωμένα docs
            _web.CoreWebView2.DOMContentLoaded -= OnDomContentLoaded;
            _web.CoreWebView2.DOMContentLoaded += OnDomContentLoaded;
        }

        private async void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!IsReady) return;
            // σε κάθε navigation ξανα-διασφαλίζουμε τους listeners
            await AddRuntimeScrollListener();
        }

        private async void OnDomContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            await AddRuntimeScrollListener();
        }

        private async Task InjectDocumentCreatedScript()
        {
            await _web.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
            (() => {
              let t;
              const post = () => chrome.webview.postMessage('scroll');
              // listener όσο νωρίτερα γίνεται
              window.addEventListener('scroll', () => {
                  clearTimeout(t);
                  t = setTimeout(post, 15);
              }, { passive:true });
            })();
        ");
        }

        private async Task AddRuntimeScrollListener()
        {
            // για ήδη φορτωμένο DOM
            string js = @"
          (function(){
            if (!window.__syncScrollInstalled) {
                window.__syncScrollInstalled = true;
                let t;
                const post = () => { try { chrome.webview.postMessage('scroll'); } catch(_) {} };
                window.addEventListener('scroll', function(){
                    clearTimeout(t); t = setTimeout(post, 15);
                }, { passive:true });
            }
            return true;
          })();";
            await SafeExec(js);
        }

        private void HookWebMessage()
        {
            if (_web.CoreWebView2 == null) return;
            _web.CoreWebView2.WebMessageReceived -= OnWebMessage;
            _web.CoreWebView2.WebMessageReceived += OnWebMessage;
        }

        private void OnWebMessage(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string s = "";
            try { s = e.TryGetWebMessageAsString(); } catch { }
            if (s == "scroll")
            {
                var h = Scrolled;
                if (h != null) h(this, EventArgs.Empty);
            }
        }

        private async Task<string> SafeExec(string js)
        {
            // Guard: αν στο μεταξύ dispose-αρίστηκε, απλώς γύρνα ""
            if (!IsReady) return "";
            try
            {
                return await _web.ExecuteScriptAsync(js);
            }
            catch
            {
                // π.χ. E_NOINTERFACE όταν ο controller δεν είναι διαθέσιμος πλέον
                return "";
            }
        }

        private static double ParseJsNumber(string jsResult)
        {
            if (jsResult == null) return 0.0;
            string s = jsResult.Trim();
            if (s.Length == 0) return 0.0;
            if (s.Length >= 2 && s[0] == '\"' && s[s.Length - 1] == '\"')
                s = s.Substring(1, s.Length - 2);
            double d;
            if (double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d))
                return d;
            return 0.0;
        }

        public void Dispose()
        {
            try
            {
                if (_web.CoreWebView2 != null)
                {
                    _web.CoreWebView2.WebMessageReceived -= OnWebMessage;
                    _web.CoreWebView2.DOMContentLoaded -= OnDomContentLoaded;
                }
            }
            catch { }
            _web.CoreWebView2InitializationCompleted -= OnInitCompleted;
            _web.NavigationCompleted -= OnNavigationCompleted;
        }
    }
 


    public sealed class ScrollSynchronizer : IDisposable
    {
        private readonly ISyncScrollAdapter _left;
        private readonly ISyncScrollAdapter _right;

        private readonly System.Windows.Forms.Timer _debounce;
        private readonly System.Windows.Forms.Timer _startTimer;

        private bool _syncing;
        private ISyncScrollAdapter _lastSource;
        private bool _initialized;

        public int DebounceMs { get; private set; }
        public int StartDelayMs { get; set; }

        public ScrollSynchronizer(ISyncScrollAdapter a, ISyncScrollAdapter b, int debounceMs)
        {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");

            _left = a;
            _right = b;

            DebounceMs = debounceMs < 0 ? 0 : debounceMs;
            StartDelayMs = 20;

            _debounce = new System.Windows.Forms.Timer();
            _debounce.Interval = DebounceMs;
            _debounce.Tick += OnDebounceTick;

            _startTimer = new System.Windows.Forms.Timer();
            _startTimer.Interval = StartDelayMs;
            _startTimer.Tick += OnStartTick;
        }

        public void Start()
        {
            _startTimer.Start();
        }

        public void Stop()
        {
            if (!_initialized) return;
            _left.Scrolled -= OnLeftScrolled;
            _right.Scrolled -= OnRightScrolled;
            _initialized = false;
        }

        private async void OnStartTick(object sender, EventArgs e)
        {
            _startTimer.Stop();

            // initialize adapters (async αλλά fire-and-forget από UI timer)
            await _left.InitializeAsync();
            await _right.InitializeAsync();

            _left.Scrolled += OnLeftScrolled;
            _right.Scrolled += OnRightScrolled;
            _initialized = true;

            // Αρχική ευθυγράμμιση όταν είναι έτοιμα και τα δύο
            if (_left.IsReady && _right.IsReady)
            {
                double r = await _left.GetRatioAsync();
                await _right.SetRatioAsync(r);
            }
            else
            {
                // retry λίγο αργότερα αν (π.χ.) το WebView2 δεν είναι έτοιμο ακόμα
                _startTimer.Interval = 100;
                _startTimer.Start();
            }
        }

        private void OnLeftScrolled(object sender, EventArgs e)
        {
            _lastSource = _left;
            _debounce.Stop();
            _debounce.Interval = DebounceMs;
            _debounce.Start();
        }

        private void OnRightScrolled(object sender, EventArgs e)
        {
            _lastSource = _right;
            _debounce.Stop();
            _debounce.Interval = DebounceMs;
            _debounce.Start();
        }

        private async void OnDebounceTick(object sender, EventArgs e)
        {
            _debounce.Stop();
            if (_syncing || !_initialized) return;
            if (_lastSource == null) return;

            ISyncScrollAdapter source = _lastSource;
            ISyncScrollAdapter target = ReferenceEquals(source, _left) ? _right : _left;

            if (!source.IsReady || !target.IsReady) return;

            try
            {
                _syncing = true;
                double ratio = await source.GetRatioAsync();
                await target.SetRatioAsync(ratio);
            }
            finally
            {
                _syncing = false;
            }
        }

        public void Dispose()
        {
            Stop();
            _debounce.Tick -= OnDebounceTick;
            _debounce.Dispose();
            _startTimer.Tick -= OnStartTick;
            _startTimer.Dispose();
        }
    }






}

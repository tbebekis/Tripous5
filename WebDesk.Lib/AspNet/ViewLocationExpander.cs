using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Razor;

namespace WebDesk.AspNet
{


    /// <summary>
    /// A themeable view location expander. It adds the views of the current theme in the view search locations.
    /// <para>Adds the following locations:</para>
    /// <para><c>/THEMES_FOLDER/THEME/Views/CONTROLLER/VIEW.cshtml</c></para>
    /// <para><c>/THEMES_FOLDER/THEME/Views/Shared/VIEW.cshtml</c></para>
    /// </summary>
    public class ViewLocationExpander : IViewLocationExpander
    {

        /* public */
        /// <summary>
        /// This is called by the <see cref="RazorViewEngine"/> when it looks to find a view (any kind of view, i.e. layout, normal view, or partial).
        /// <para>Information about the view, action and controller is passed in the <see cref="ViewLocationExpanderContext"/> parameter.</para>
        /// <para>This call gives a chance to search and locate the view and pass back the view location.</para>
        /// <para>NOTE: The order of view location matters.</para>
        /// </summary>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            /*
                        // 0 = view file name
                        // 1 = controller name
                        if (context.Values.TryGetValue(SThemeKey, out string Theme))
                        {
                            var Locations = new[] {
                                    $"/{ThemesFolder}/{Theme}/Views/{{1}}/{{0}}.cshtml",
                                    $"/{ThemesFolder}/{Theme}/Views/Shared/{{0}}.cshtml",
                                };

                            viewLocations = Locations.Union(viewLocations);
                        } 
             */

            return viewLocations;
        }
        /// <summary>
        /// More or less useless. 
        /// <para>It is used in adding values to the <see cref="ViewLocationExpanderContext.Values"/> dictionary 
        /// in order to be available when the  <see cref="ExpandViewLocations(ViewLocationExpanderContext, IEnumerable{string})"/> method is called. </para>
        /// </summary>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            /*
                        if (!IsNonThemeableArea(context.AreaName)) // some areas, such as Admin, may be not themeable
                        {
                            context.Values[SThemeKey] = Theme;
                        } 
             */
        }

    }


}

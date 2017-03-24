namespace Sitecore.ControllerLayout.Extensions
{
    using System.Linq;
    using Data;
    using Data.Items;

    /// <summary>
    /// Extensions for sitecore TemplateItem's
    /// </summary>
    public static class TemplateExtensions
    {
        /// <summary>
        /// Descendses from.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <returns></returns>
        public static bool DescendsFrom(this TemplateItem template, string templateId)
        {
            return DescendsFrom(template, ID.Parse(templateId));
        }

        /// <summary>
        /// Descendses from.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <returns></returns>
        public static bool DescendsFrom(this TemplateItem template, ID templateId)
        {
            if (template == null)
            {
                return false;
            }

            if (template.ID == templateId)
            {
                return true;
            }
            return template.ID != Sitecore.TemplateIDs.StandardTemplate && template.BaseTemplates.Any(baseTemplate => baseTemplate.DescendsFrom(templateId));
        }
    }
}
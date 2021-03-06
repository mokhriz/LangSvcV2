﻿namespace Tvl.VisualStudio.Language.Antlr3.Project
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using IExtenderProvider = EnvDTE.IExtenderProvider;
    using IExtenderSite = EnvDTE.IExtenderSite;
    using IInternalExtenderProvider = EnvDTE80.IInternalExtenderProvider;
    using PropertyDescriptor = System.ComponentModel.PropertyDescriptor;
    using PropertyDescriptorCollection = System.ComponentModel.PropertyDescriptorCollection;
    using TypeDescriptor = System.ComponentModel.TypeDescriptor;

    [Guid("2FBDD105-8744-4642-BBFB-679C4611A442")]
    [ComVisible(true)]
    public class GrammarFileObjectExtenderProvider : IExtenderProvider, IInternalExtenderProvider
    {
        public static readonly string Name = "AntlrGrammarFileObjectExtender";

        // Only allow 1 extender at a time to ensure the NotifyDelete is always called
        private object _extender;
        private IExtenderSite _extenderSite;
        private int _cookie;

        #region IExtenderProvider Members

        public bool CanExtend(string ExtenderCATID, string ExtenderName, object ExtendeeObject)
        {
            return GetExtendVersion(ExtenderCATID, ExtenderName, ExtendeeObject) != ExtendVersion.None;
        }

        public object GetExtender(string ExtenderCATID, string ExtenderName, object ExtendeeObject, IExtenderSite ExtenderSite, int Cookie)
        {
            object extender = CreateExtender(ExtenderCATID, ExtenderName, ExtendeeObject, ExtenderSite, Cookie);
            if (extender != null)
            {
                try
                {
                    if (_extenderSite != null && _cookie != 0)
                    {
                        _extenderSite.NotifyDelete(_cookie);
                    }
                }
                catch (Exception ex) when (!ex.IsCritical())
                {
                }

                _extender = extender;
                _extenderSite = ExtenderSite;
                _cookie = Cookie;
            }

            return extender;
        }

        protected object CreateExtender(string ExtenderCATID, string ExtenderName, object ExtendeeObject, IExtenderSite ExtenderSite, int Cookie)
        {
            object extender = null;

            ExtendVersion extendVersion = GetExtendVersion(ExtenderCATID, ExtenderName, ExtendeeObject);
            if (extendVersion != ExtendVersion.None)
            {
                IVsBrowseObject browseObject = ExtendeeObject as IVsBrowseObject;
                if (browseObject == null)
                    return null;

                IVsHierarchy hierarchy;
                uint itemId;
                if (ErrorHandler.Failed(browseObject.GetProjectItem(out hierarchy, out itemId)))
                    return null;

                IVsBuildPropertyStorage buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;
                if (buildPropertyStorage == null)
                    return null;

                if (extendVersion == ExtendVersion.Antlr3)
                    extender = new GrammarFileObjectExtenderProperties(buildPropertyStorage, itemId);
                else
                    extender = new GrammarFileObjectExtenderPropertiesV4(buildPropertyStorage, itemId);
            }

            return extender;
        }

        #endregion

        #region IInternalExtenderProvider Members

        public object GetExtenderNames(string ExtenderCATID, object ExtendeeObject)
        {
            return new string[] { Name };
        }

        #endregion

        private ExtendVersion GetExtendVersion(string ExtenderCATID, string ExtenderName, object ExtendeeObject)
        {
            if (ExtenderName != Name)
                return ExtendVersion.None;

            try
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(ExtendeeObject);
                if (properties == null)
                    return ExtendVersion.None;

                PropertyDescriptor property = properties["ItemType"];
                if (property == null)
                    return ExtendVersion.None;

                object value = property.GetValue(ExtendeeObject);
                if (value == null)
                    return ExtendVersion.None;

                string itemType = value.ToString();
                if (string.Equals(itemType, "Antlr3", StringComparison.OrdinalIgnoreCase))
                    return ExtendVersion.Antlr3;

                if (string.Equals(itemType, "Antlr4", StringComparison.OrdinalIgnoreCase))
                    return ExtendVersion.Antlr4;

                return ExtendVersion.None;
            }
            catch (Exception ex) when (!ex.IsCritical())
            {
                return ExtendVersion.None;
            }
        }

        private enum ExtendVersion
        {
            None,
            Antlr3,
            Antlr4,
        }
    }
}

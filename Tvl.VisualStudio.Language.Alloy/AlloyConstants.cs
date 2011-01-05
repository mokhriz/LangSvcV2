﻿namespace Tvl.VisualStudio.Language.Alloy
{
    using System;

    public static class AlloyConstants
    {
        public const string AlloyLanguageName = "Alloy";
        public const string AlloyContentType = "Alloy";
        public const string AlloyFileExtension = ".als";

        // product registration
        public const int AlloyLanguageResourceId = 100;
        public const string AlloyLanguagePackageNameResourceString = "#110";
        public const string AlloyLanguagePackageDetailsResourceString = "#111";
        public const string AlloyLanguagePackageProductVersionString = "1.0";

        public const string AntlrIntellisenseOutputWindow = "ANTLR IntelliSense Engine";

        public const string AlloyLanguagePackageGuidString = "E18267CE-5A78-46DF-87AE-21891D8FB2DE";
        public static readonly Guid AlloyLanguagePackageGuid = new Guid("{" + AlloyLanguagePackageGuidString + "}");

        public const string AlloyLanguageGuidString = "886D06A5-E920-44F0-9271-4E8221907DF1";
        public static readonly Guid AlloyLanguageGuid = new Guid("{" + AlloyLanguageGuidString + "}");

        public const string UIContextNoSolution = "ADFC4E64-0397-11D1-9F4E-00A0C911004F";
        public const string UIContextSolutionExists = "f1536ef8-92ec-443c-9ed7-fdadf150da82";
    }
}

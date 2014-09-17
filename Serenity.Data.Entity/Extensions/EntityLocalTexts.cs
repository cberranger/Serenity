
namespace Serenity.Localization
{
    using Localization;
    using Serenity.Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    public static class EntityLocalTexts
    {
        public static void Initialize(string languageID = LocalText.InvariantLanguageID)
        {
            var provider = Dependency.Resolve<ILocalTextRegistry>();

            foreach (var row in RowRegistry.EnumerateRows())
            {
                var fields = row.GetFields();
                var prefix = fields.LocalTextPrefix;

                foreach (var field in row.GetFields())
                {
                    LocalText lt = field.Caption;
                    if (lt != null &&
                        !lt.Key.IsEmptyOrNull() &&
                        !lt.Key.StartsWith("Db."))
                    {
                        var key = "Db." + prefix + "." + (field.PropertyName ?? field.Name);
                        provider.Add(languageID, key, lt.Key);
                        field.Caption = new LocalText(key);
                    }
                }

                var displayName = row.GetType().GetCustomAttribute<DisplayNameAttribute>();
                if (displayName != null)
                    provider.Add(languageID, "Db." + prefix + ".EntityPlural", displayName.DisplayName);

                var instanceName = row.GetType().GetCustomAttribute<InstanceNameAttribute>();
                if (instanceName != null)
                    provider.Add(languageID, "Db." + prefix + ".EntitySingular", instanceName.InstanceName);
            }
        }

    }
}
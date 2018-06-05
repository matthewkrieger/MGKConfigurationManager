using System;
using System.Collections.Specialized;
using System.Linq;
using System.Configuration;

public class MGKConfigurationManager
{
    private static readonly Lazy<MGKConfigurationManager> lazy = new Lazy<MGKConfigurationManager>(() => new MGKConfigurationManager());

    public static MGKConfigurationManager Instance { get { return lazy.Value; } }

    public MGKConfigurationManager(params string[] sectionNames)
    {
        LoadStandardAppSettings();
        LoadCustomSections(sectionNames);
    }

    private NameValueCollection _appSettings = new NameValueCollection();

    public NameValueCollection AppSettings
    {
        get
        {
            return _appSettings;
        }
    }

    private void LoadStandardAppSettings()
    {
        var items = ConfigurationManager.AppSettings.AllKeys.SelectMany(ConfigurationManager.AppSettings.GetValues, (k, v) => new { key = k, value = v });
        foreach (var item in items)
        {
            _appSettings[item.key] = item.value;
        }
    }

    private void LoadCustomSections(string[] sections)
    {
        foreach(string s in sections)
        {
            var section = (NameValueCollection)ConfigurationManager.GetSection(s);
            var items = section.AllKeys.SelectMany(section.GetValues, (k, v) => new { key = k, value = v });
            foreach (var item in items)
            {
                _appSettings[item.key] = item.value;
            }
        }
    }
}

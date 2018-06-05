# MGKConfigurationManager
Simple configuration manager for settings and secrets in .NET

MGKConfigurationManager is a simple .NET library for pulling in per-project and solution-wide secrets and solution-wide settings when developing locally.  (Per-project settings will be in web.config as normal.)  When running in a hosted environment (e.g. Microsoft Azure) settings and secrets would typically be stored in app configuration settings or a dedicated key vault.

All settings and secrets are stored outside the project working directory so as not to be checked into source control.

MGKConfigurationManager imports all settings and secrets into a single, flat namespace implemented as a .NET `NameValueCollection`.  As a result key names must be unique across all config files.

MGKConfigurationManager has no error or exception handling.

**Configuration:**

Place the following in the `<configSections>` element of your `web.config` file in each project within a given solution:
  
```
<configSections>
  <section name="LocalSecrets" type="System.Configuration.NameValueFileSectionHandler, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
  <section name="GlobalSecrets" type="System.Configuration.NameValueFileSectionHandler, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
  <section name="GlobalSettings" type="System.Configuration.NameValueFileSectionHandler, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
</configSections>
```

Place the following directly under the `<configuration>` element of your `web.config` file in each project within a given solution:
  
```
<LocalSecrets file="..\..\{SOLUTION_NAME}.Settings\{PROJECT_NAME}\localsecrets.config"></LocalSecrets>
<GlobalSecrets file="..\..\{SOLUTION_NAME}.Settings\globalsecrets.config"></GlobalSecrets>
<GlobalSettings file="..\..\{SOLUTION_NAME}.Settings\globalsettings.config"></GlobalSettings>
```

where:

* `{SOLUTION_NAME}` is the name of your overall solution
* `{PROJECT_NAME}` is the name of the project where the `web.config` file resides

`localsecrets.config`, `globalsecrets.config` and `globalsettings.config` look as follows:

`localsecrets.config`:
```
<LocalSecrets>
  <add key="key1" value="value1" />
  <add key="key2" value="value2" />
</LocalSecrets>
```

`globalsecrets.config`:
```
<GlobalSecrets>
  <add key="key1" value="value1" />
  <add key="key2" value="value2" />
</GlobalSecrets>
```

`globalsettings.config`:
```
<GlobalSettings>
  <add key="key1" value="value1" />
  <add key="key2" value="value2" />
</GlobalSettings>
```

**Sample directory structure:**

```
\My_solution_dir
  \Project1
    web.config
  \Project2
    web.config
  \Project3
  My_solution.sln
\My_solution.Settings
  globalsecrets.config
  globalsettings.config
  \Project1
    localsecrets.config
  \Project2
    localsecrets.config
  \Project3
    localsecrets.config
```

**Usage:**

Bootstrap MGKConfigurationManager as follows - when your app starts (entry point will depend on project type), create a static instance of `MGKConfigurationManager` passing an array of custom section names as shown above.

`static MGKConfigurationManager settings = new MGKConfigurationManager(new string[] { "LocalSecrets", "GlobalSecrets", "GlobalSettings" });`

As stated above, MGKConfigurationManager imports all settings and secrets into a single, flat namespace where key name uniqueness is a requirement.  Values can be retrieved as follows:

`settings.AppSettings["My_setting_or_secret_name"]);`

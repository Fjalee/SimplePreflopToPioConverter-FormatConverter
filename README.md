# SimplePreflopToPioConverter-FormatConverter
previous project's SimplePreflopToPioConverter output format converter

* repo to convert format inside the .txt files https://github.com/Fjalee/SimplePreflopToPioConverter

## How to use 
* local-appsettings.json
   * create file 'local-appsettings.json' in the same directory as appsettings.json, and change your-input-directory and your-output-directory accordingly
```
{
  "AppSettings": {
    "InputDir": "C:\\Users\\YourUser\\your-input-directory",
    "OutputDir": "C:\\Users\\YourUser\\your-output-directory"
  }
}
```

* appsettings.json
   * adjust "OutputPatterns" and "InputPatterns" objects accordingly

# ?? CSV to Excel Converter (Console + WinForms)

A lightweight tool that converts CSV files into clean Excel `.xlsx` files.  
This project includes both **Console** and **WinForms UI** versions to demonstrate practical data?processing capabilities.

---

## ? Features

- Convert CSV ¡æ Excel with one click  
- Auto?fit Excel columns  
- Trim whitespace from fields  
- WinForms UI with:
  - CSV file picker
  - Excel output file picker
  - Log panel showing progress  
- Includes sample CSV for testing

---

## ?? Tech Stack

- **C# / .NET**
- **ClosedXML** for Excel generation  
- **WinForms** (UI version)
- **File IO, DataTable, basic CSV parsing**

---

## ?? Project Structure

/csv-excel-converter
/ConsoleVersion
/WinFormsVersion
/sample
test.csv
result.xlsx (generated)
/screenshots
01-main.png
02-selected-file.png
03-finished.png

---

## ¢º How to Use (WinForms)

1. Launch `CsvExcelConverter.WinForms.exe`
2. Click **Browse** to select a CSV file  
3. Excel output path auto?fills  
4. Press **Convert**
5. Check the output `.xlsx` file  

---

## ¢º How to Use (Console)

dotnet run

Default paths:
sample/test.csv
sample/result.xlsx

---

## ?? Screenshots

| Main Window | After Selecting CSV | Conversion Completed |
|------------|---------------------|----------------------|
| ![](screenshots/01-main.png) | ![](screenshots/02-selected-file.png) | ![](screenshots/03-finished.png) |

---

## ?? Sample CSV (included)

```csv
Name,Email,Age,Country,IsActive
Alice,alice@example.com,30,USA,true
Bob,bob@example.com,,Canada,false
Charlie , charlie@example.com , 35 , South Korea , true
"David, Jr.",david.jr@example.com,28,UK,true
Eve,eve@example.com,41,USA,false
```

---

## ? Limitations

- Simple CSV parser (does not fully support quoted comma fields)
- No type conversion (all fields are string)

---

## ?? Future Improvements

- Add CsvHelper for robust parsing
- Batch multi?file conversion
- Column type detection
- Add dark/light UI themes

---

## ?? License

MIT License




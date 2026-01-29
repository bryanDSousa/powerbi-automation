using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

var columnIndex = new Dictionary<string, List<string>>();

// Recorremos todas las tablas y columnas
foreach (var table in Model.Tables)
{
    foreach (var column in table.Columns)
    {
        var normalizedName = column.Name
            .Trim()
            .ToLowerInvariant();

        if (!columnIndex.ContainsKey(normalizedName))
        {
            columnIndex[normalizedName] = new List<string>();
        }

        columnIndex[normalizedName].Add(
            table.Name + "." + column.Name
        );
    }
}

// Columnas ambiguas (mismo nombre en varias tablas)
var ambiguousColumns = columnIndex
    .Where(kvp => kvp.Value.Count > 1)
    .OrderBy(kvp => kvp.Key)
    .ToList();

// Ruta del fichero
var filePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    "TabularEditor_ColumnAmbiguity_Report.txt"
);

// Escritura del fichero
using (var writer = new StreamWriter(filePath, false))
{
    writer.WriteLine("TABULAR EDITOR – COLUMN AMBIGUITY REPORT");
    writer.WriteLine("Generated: " + DateTime.Now);
    writer.WriteLine(new string('-', 50));
    writer.WriteLine();

    if (ambiguousColumns.Any())
    {
        foreach (var item in ambiguousColumns)
        {
            writer.WriteLine("Column name: " + item.Key);
            foreach (var location in item.Value)
            {
                writer.WriteLine("  - " + location);
            }
            writer.WriteLine();
        }
    }
    else
    {
        writer.WriteLine("No ambiguous column names detected.");
    }
}

// Mensaje final en Output
Info("✅ Analysis completed.");
Info("📄 Report generated at:");
Info(filePath);

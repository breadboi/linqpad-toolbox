<Query Kind="Program" />

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
	private static readonly string TailwindCDN = @"<link href=""https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css"" rel=""stylesheet"">";

	// Write custom extension methods here. They will be available to all queries.
	public static void RenderTailwindHtml(string htmlContent)
	{
		// Combine the Tailwind CDN and HTML content
		string fullHtml = TailwindCDN + htmlContent;
		Util.RawHtml(fullHtml).Dump();
	}

	// GridItem class to hold content and its column span
    public class GridItem
    {
        public string Content { get; set; }
        public int ColumnSpan { get; set; } = 1;

        public GridItem(string content, int columnSpan = 1)
        {
            Content = content;
            ColumnSpan = columnSpan;
        }
    }

    /// <summary>
    /// Renders a grid layout with specified columns and items.
    /// </summary>
    public static void RenderGrid(int columns, params GridItem[] items)
    {
        string gridTemplateColumns = $"grid-cols-{columns}";
        string gridItems = string.Join("", items.Select(item => $"<div class='col-span-{item.ColumnSpan}'>{item.Content}</div>"));
        string htmlContent = $@"
<div class='grid {gridTemplateColumns} gap-4 m-4'>
  {gridItems}
</div>
";
        RenderTailwindHtml(htmlContent);
    }

    // Modify existing methods to return HTML strings
    public static string GetCardContent(string title, string content)
    {
        return $@"
<div class='max-w-sm rounded overflow-hidden shadow-lg'>
  <div class='px-6 py-4'>
    <div class='font-bold text-xl mb-2'>{title}</div>
    <p class='text-gray-700 text-base'>
      {content}
    </p>
  </div>
</div>
";
    }

    public static void RenderCard(string title, string content)
    {
        RenderTailwindHtml(GetCardContent(title, content));
    }

    public static string GetTableContent<T>(IEnumerable<T> data)
    {
        var properties = typeof(T).GetProperties();

        var headerRow = string.Join("", properties.Select(p => $"<th class='px-4 py-2 border'>{p.Name}</th>"));
        var rows = string.Join("", data.Select(item => 
            $"<tr>{string.Join("", properties.Select(p => $"<td class='border px-4 py-2'>{p.GetValue(item)}</td>"))}</tr>"));

        return $@"
<table class='table-auto border-collapse w-full'>
  <thead>
    <tr>
      {headerRow}
    </tr>
  </thead>
  <tbody>
    {rows}
  </tbody>
</table>
";
    }

    public static void RenderTable<T>(IEnumerable<T> data)
    {
        RenderTailwindHtml(GetTableContent(data));
    }

    public static string GetListContent(IEnumerable<string> items)
    {
        var listItems = string.Join("", items.Select(item => $"<li class='mb-2'>{item}</li>"));
        return $@"
<ul class='list-disc list-inside'>
  {listItems}
</ul>
";
    }

    public static void RenderList(IEnumerable<string> items)
    {
        RenderTailwindHtml(GetListContent(items));
    }

    public static string GetAlertContent(string message, string alertType = "info")
    {
        string bgColor = alertType switch
        {
            "success" => "bg-green-100 border-green-500 text-green-700",
            "warning" => "bg-yellow-100 border-yellow-500 text-yellow-700",
            "error" => "bg-red-100 border-red-500 text-red-700",
            _ => "bg-blue-100 border-blue-500 text-blue-700", // info
        };

        return $@"
<div class='border-l-4 {bgColor} p-4' role='alert'>
  <p class='font-bold'>{alertType.ToUpper()}</p>
  <p>{message}</p>
</div>
";
    }

    public static void RenderAlert(string message, string alertType = "info")
    {
        RenderTailwindHtml(GetAlertContent(message, alertType));
    }

    public static string GetCodeSnippetContent(string code, string language = "csharp")
    {
        return $@"
<pre class='bg-gray-100 rounded p-4 overflow-auto'>
  <code class='language-{language}'>{System.Net.WebUtility.HtmlEncode(code)}</code>
</pre>
";
    }

    public static void RenderCodeSnippet(string code, string language = "csharp")
    {
        RenderTailwindHtml(GetCodeSnippetContent(code, language));
    }

	public static string GetCollapsibleContent(string header, string content)
	{
		return $@"
<details class='bg-gray-100 rounded border border-gray-200 p-4'>
  <summary class='font-semibold'>{header}</summary>
  <div class='mt-2'>
    {content}
  </div>
</details>
";
	}

	public static void RenderCollapsible(string header, string content)
	{
		RenderTailwindHtml(GetCollapsibleContent(header, content));
	}

	public static string GetHeaderContent(string text, int level = 1)
	{
		string tag = $"h{level}";
		string textSize = level switch
		{
			1 => "text-4xl",
			2 => "text-3xl",
			3 => "text-2xl",
			4 => "text-xl",
			5 => "text-lg",
			_ => "text-base",
		};

		return $@"
<{tag} class='{textSize} font-bold'>
  {text}
</{tag}>
";
	}

	public static void RenderHeader(string text, int level = 1)
	{
		RenderTailwindHtml(GetHeaderContent(text, level));
	}

	public static string GetParagraphContent(string text)
	{
		return $@"
<p class='text-base text-gray-800'>
  {text}
</p>
";
	}

	public static void RenderParagraph(string text)
	{
		RenderTailwindHtml(GetParagraphContent(text));
	}
}

// You can also define namespaces, non-static classes, enums, etc.

#region Advanced - How to multi-target

// The NETx symbol is active when a query runs under .NET x or later.

#if NET7
// Code that requires .NET 7 or later
#endif

#if NET6
// Code that requires .NET 6 or later
#endif

#if NET5
// Code that requires .NET 5 or later
#endif

#endregion
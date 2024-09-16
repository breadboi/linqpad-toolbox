<Query Kind="Program" />

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
	private static readonly string TailwindCDN = @"
<link href=""https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css"" rel=""stylesheet"">
<script>
function openModal() {
    document.getElementById('modal').classList.remove('hidden');
}
function closeModal() {
    document.getElementById('modal').classList.add('hidden');
}
</script>";

	public static void RenderTailwindHtml(string htmlContent)
	{
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

	public static string GetGridContent(int columns, GridItem[] items)
	{
		string gridTemplateColumns = $"grid-cols-{columns}";
		string gridItems = string.Join("", items.Select(item => $"<div class='col-span-{item.ColumnSpan}'>{item.Content}</div>"));
		return $@"
<div class='grid {gridTemplateColumns} gap-4 mb-6'>
  {gridItems}
</div>
";
	}

	public static void RenderGrid(int columns, params GridItem[] items)
	{
		RenderTailwindHtml(GetGridContent(columns, items));
	}

	public static string GetBadgeContent(string text, string variant = "secondary")
	{
		string bgColor = variant switch
		{
			"secondary" => "bg-gray-200 text-gray-800",
			"primary" => "bg-blue-500 text-white",
			"success" => "bg-green-500 text-white",
			"warning" => "bg-yellow-500 text-white",
			"error" => "bg-red-500 text-white",
			_ => "bg-gray-200 text-gray-800"
		};

		return $"<span class='px-2 py-1 rounded {bgColor} m-1 text-sm'>{text}</span>";
	}

	public static void RenderBadge(string text, string variant = "secondary")
	{
		RenderTailwindHtml(GetBadgeContent(text, variant));
	}

	public static string GetCardContent(string title, string content)
	{
		return $@"
<div class='rounded overflow-hidden shadow-lg mb-6 border border-gray-200'>
  <div class='px-6 py-4 bg-gray-100'>
    <div class='font-bold text-xl mb-2'>{title}</div>
  </div>
  <div class='px-6 py-4'>
    {content}
  </div>
</div>
";
	}

	public static void RenderCard(string title, string content)
	{
		RenderTailwindHtml(GetCardContent(title, content));
	}

	public static void RenderCard(string title, Func<string> contentFunc)
	{
		string content = contentFunc();
		RenderTailwindHtml(GetCardContent(title, content));
	}

	public static string GetTableContent<T>(IEnumerable<T> data)
	{
		var properties = typeof(T).GetProperties();

		var headerRow = string.Join("", properties.Select(p => $"<th class='px-4 py-2 border-b-2'>{p.Name}</th>"));
		var rows = string.Join("", data.Select(item =>
			$"<tr>{string.Join("", properties.Select(p => $"<td class='border-t px-4 py-2'>{p.GetValue(item)}</td>"))}</tr>"));

		return $@"
<table class='table-auto border-collapse w-full text-left'>
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

	public static string GetCollapsibleTableContent<T>(string title, IEnumerable<T> data)
	{
		string tableContent = GetTableContent(data);
		return GetCollapsibleContent(title, tableContent);
	}

	public static void RenderCollapsibleTable<T>(string title, IEnumerable<T> data)
	{
		RenderTailwindHtml(GetCollapsibleTableContent(title, data));
	}

	public static string GetListContent(IEnumerable<string> items)
	{
		var listItems = string.Join("", items.Select(item => $"<li class='mb-1'>{item}</li>"));
		return $@"
<ul class='list-disc list-inside mb-4'>
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
<div class='border-l-4 {bgColor} p-4 mb-4' role='alert'>
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
<pre class='bg-gray-100 rounded p-4 overflow-auto mb-4'>
  <code class='language-{language}'>{System.Net.WebUtility.HtmlEncode(code)}</code>
</pre>
";
	}

	public static void RenderCodeSnippet(string code, string language = "csharp")
	{
		RenderTailwindHtml(GetCodeSnippetContent(code, language));
	}

	public static string GetCollapsibleContent(string header, string content, bool isOpen = false)
	{
		string openAttribute = isOpen ? "open" : "";
		return $@"
<details class='details-reset mb-4' {openAttribute}>
  <summary class='font-semibold text-lg mb-2'>{header}</summary>
  <div class='ml-4'>
    {content}
  </div>
</details>
";
	}

	public static void RenderCollapsible(string header, string content, bool isOpen = false)
	{
		RenderTailwindHtml(GetCollapsibleContent(header, content, isOpen));
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
<{tag} class='{textSize} font-bold mb-4'>
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
<p class='text-base text-gray-800 mb-4'>
  {text}
</p>
";
	}

	public static void RenderParagraph(string text)
	{
		RenderTailwindHtml(GetParagraphContent(text));
	}

	// Additional methods for Tailwind CSS features

	public static string GetButtonContent(string text, string variant = "primary", string onClick = "")
	{
		string bgColor = variant switch
		{
			"primary" => "bg-blue-500 hover:bg-blue-700 text-white m-2",
			"secondary" => "bg-gray-500 hover:bg-gray-700 text-white m-2",
			"success" => "bg-green-500 hover:bg-green-700 text-white m-2",
			"warning" => "bg-yellow-500 hover:bg-yellow-700 text-white m-2",
			"error" => "bg-red-500 hover:bg-red-700 text-white m-2",
			_ => "bg-gray-500 hover:bg-gray-700 text-white m-2"
		};

		string onClickAttribute = string.IsNullOrWhiteSpace(onClick) ? "" : $"onclick='{onClick}'";

		return $@"
<button class='px-4 py-2 rounded {bgColor}' {onClickAttribute}>
  {text}
</button>
";
	}


	public static void RenderButton(string text, string variant = "primary")
	{
		RenderTailwindHtml(GetButtonContent(text, variant));
	}

	public static string GetModalContent(string title, string content)
	{
		return $@"
<div id='modal' class='fixed z-10 inset-0 overflow-y-auto hidden'>
  <div class='flex items-center justify-center min-h-screen'>
    <div class='bg-white rounded-lg overflow-hidden shadow-xl transform transition-all max-w-lg w-full'>
      <div class='bg-gray-100 px-4 py-3'>
        <h3 class='text-lg leading-6 font-medium text-gray-900'>
          {title}
        </h3>
      </div>
      <div class='px-4 py-5'>
        {content}
      </div>
      <div class='bg-gray-100 px-4 py-3 flex justify-end'>
        <button onclick='closeModal()' class='px-4 py-2 bg-blue-500 text-white rounded'>Close</button>
      </div>
    </div>
  </div>
</div>
";
	}


	public static void RenderModal(string title, string content)
	{
		RenderTailwindHtml(GetModalContent(title, content));
	}

	// Add more methods as needed for other Tailwind CSS features...
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

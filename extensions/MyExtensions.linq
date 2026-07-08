<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
	private static readonly string TailwindCDN = @"
<link href=""https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css"" rel=""stylesheet"">
<link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/themes/prism.min.css"" />
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/prism.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/components/prism-xml.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/components/prism-json.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/components/prism-csharp.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/components/prism-sql.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/components/prism-javascript.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/components/prism-css.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/prism/1.25.0/components/prism-markup.min.js""></script>
<style>
/* Custom CSS for collapsible caret rotation */
.details-reset[open] > summary svg {
    transform: rotate(90deg);
}

/* Animation for expand/collapse */
.transition-max-height {
    transition: max-height 0.3s ease;
}

/* Syntax highlighting override */
pre {
    max-height: 70vh;
    overflow: auto;
}

/* Add spacing between collapsible sections */
.mb-6 {
    margin-bottom: 1.5rem;
}

/* Menu styles */
#sideMenu {
    /* Add a semi-transparent overlay */
    background-color: rgba(255, 255, 255, 0.95);
}

#final {
	padding: 15px;
}

</style>
<script>
function openModal(id) {
    document.getElementById(id).classList.remove('hidden');
    var codeBlocks = document.getElementById(id).querySelectorAll('pre code');
    codeBlocks.forEach((block) => {
        Prism.highlightElement(block);
    });
}
function closeModal(id) {
    document.getElementById(id).classList.add('hidden');
}
function toggleCollapsible(id, headerElement) {
    var content = document.getElementById(id);
    if (content.classList.contains('hidden')) {
        content.classList.remove('hidden');
        headerElement.querySelector('svg').classList.add('rotate-90');
    } else {
        content.classList.add('hidden');
        headerElement.querySelector('svg').classList.remove('rotate-90');
    }
    event.stopPropagation();
}

function toggleMenu() {
    var menu = document.getElementById('sideMenu');
    if (menu.classList.contains('translate-x-full')) {
        menu.classList.remove('translate-x-full');
    } else {
        menu.classList.add('translate-x-full');
    }
}
</script>
";

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

		string htmlContent = $@"
<{tag} id='{text.Replace(" ", "").ToLower()}' class='{textSize} font-bold mb-4 mt-8'>
  {text}
</{tag}>
";
		RenderTailwindHtml(htmlContent);
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

	public static void RenderCustomCollapsible(string header, string content)
	{
		RenderTailwindHtml(header + content);
	}

	public static void RenderPageHeader(List<string> sectionTitles)
	{
		// Build the header HTML
		var headerBuilder = new StringBuilder();

		headerBuilder.AppendLine("<div id='pageHeader' class='bg-gray-800 text-white p-4 fixed top-0 left-0 right-0 z-50 transition-transform'>");
		headerBuilder.AppendLine("<div class='container mx-auto'>");
		headerBuilder.AppendLine("<div class='flex space-x-4'>");

		foreach (var title in sectionTitles)
		{
			var anchor = title.Replace(" ", "").ToLower();
			headerBuilder.AppendLine($"<a href='#{anchor}' class='hover:underline'>{title}</a>");
		}

		headerBuilder.AppendLine("</div>");
		headerBuilder.AppendLine("</div>");
		headerBuilder.AppendLine("</div>");

		// Add placeholder for content offset
		headerBuilder.AppendLine("<div style='height:60px;'></div>");

		RenderTailwindHtml(headerBuilder.ToString());
	}

	public static void RenderHamburgerMenu(List<string> sectionTitles)
	{
		var menuBuilder = new StringBuilder();

		// Hamburger Menu Icon
		menuBuilder.AppendLine(@"
<!-- Hamburger Menu -->
<div class='fixed top-0 right-0 m-4 z-50'>
  <button onclick='toggleMenu()' class='text-gray-800 focus:outline-none'>
    <!-- Hamburger icon -->
    <svg class='w-8 h-8' fill='none' stroke='currentColor' viewBox='0 0 24 24'>
      <path id='hamburgerIcon' stroke-linecap='round' stroke-linejoin='round' stroke-width='2'
        d='M4 6h16M4 12h16M4 18h16'></path>
    </svg>
  </button>
</div>

<!-- Side Navigation Menu -->
<div id='sideMenu' class='fixed top-0 right-0 h-full w-64 bg-white shadow-lg transform translate-x-full transition-transform duration-300 z-40'>
  <div class='p-4'>
    <button onclick='toggleMenu()' class='text-gray-800 focus:outline-none mb-4'>
      <!-- Close icon -->
      <svg class='w-6 h-6' fill='none' stroke='currentColor' viewBox='0 0 24 24'>
        <path stroke-linecap='round' stroke-linejoin='round' stroke-width='2'
          d='M6 18L18 6M6 6l12 12'></path>
      </svg>
    </button>
    <!-- Navigation Links -->
    <nav class='space-y-2'>
");

		foreach (var title in sectionTitles)
		{
			var anchor = title.Replace(" ", "").ToLower();
			menuBuilder.AppendLine($"<a href='#{anchor}' onclick='toggleMenu()' class='block text-gray-800 hover:underline'>{title}</a>");
		}

		menuBuilder.AppendLine(@"
    </nav>
  </div>
</div>
");

		RenderTailwindHtml(menuBuilder.ToString());
	}
	// Add more methods as needed for other Tailwind CSS features...
}

// You can also define namespaces, non-static classes, enums, etc.
public static class AIExtensions
{
	public static async Task<string> GetOpenAIResponse(string prompt)
	{
		string apiKey = Util.GetPassword("openai.apikey");

		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
			client.Timeout = Timeout.InfiniteTimeSpan;  // Some requests can run long.

			var requestBody = new
			{
				model = "gpt-4o",
				input = prompt,
				tools = new[]
				{
				new { type = "web_search_preview" }
			},
				temperature = 0.7
			};

			var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PostAsync("https://api.openai.com/v1/responses", content);

			if (response.IsSuccessStatusCode)
			{
				var responseString = await response.Content.ReadAsStringAsync();
				using var document = JsonDocument.Parse(responseString);
				var root = document.RootElement;

				if (root.TryGetProperty("output", out JsonElement outputArray))
				{
					foreach (var outputItem in outputArray.EnumerateArray())
					{
						if (outputItem.TryGetProperty("type", out JsonElement typeElement) &&
							typeElement.GetString() == "message" &&
							outputItem.TryGetProperty("content", out JsonElement contentArray))
						{
							foreach (var contentItem in contentArray.EnumerateArray())
							{
								if (contentItem.TryGetProperty("type", out JsonElement contentType) &&
									contentType.GetString() == "output_text" &&
									contentItem.TryGetProperty("text", out JsonElement textElement))
								{
									return textElement.GetString();
								}
							}
						}
					}
				}

				throw new Exception("No message content found in the response.");
			}
			else
			{
				var error = await response.Content.ReadAsStringAsync();
				throw new Exception($"OpenAI API Error: {error}");
			}
		}
	}

	// ---------------------------------------------------------
	// Loads the JSON cache file into a Dictionary<string, string>
	// If the file doesn't exist or is empty, returns a new dictionary.
	// ---------------------------------------------------------
	//public static Dictionary<string, string> LoadOpenAiCache(string cachePath)
	//{
	//	if (!File.Exists(cachePath))
	//		return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	//	try
	//	{
	//		var json = File.ReadAllText(cachePath);
	//		var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
	//		return dict ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	//	}
	//	catch
	//	{
	//		// If corrupt, just return empty
	//		return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	//	}
	//}

	// ---------------------------------------------------------
	// Saves the given dictionary back to the cache JSON file
	// ---------------------------------------------------------
	//public static void SaveOpenAiCache(Dictionary<string, string> cache, string cachePath)
	//{
	//	var json = JsonConvert.SerializeObject(cache, Newtonsoft.Json.Formatting.Indented);
	//	File.WriteAllText(cachePath, json);
	//}

	// ---------------------------------------------------------
	// Uses OpenAI to classify a "candidate column name" into one
	// of the known field names.  If none is suitable, returns "Unknown".
	// The 'promptTemplate' should contain placeholders:
	//   {0} -> string.Join(", ", knownFields)
	//   {1} -> candidateCol
	// ---------------------------------------------------------
	//public static string ClassifyColumnNameWithOpenAi(
	//	string candidateCol,
	//	List<string> knownFields,
	//	Dictionary<string, string> cache,
	//	string cachePath,
	//	string promptTemplate)
	//{
	//	// 1) Check cache first
	//	if (cache.TryGetValue(candidateCol, out var cachedClassification))
	//		return cachedClassification;

	//	// 2) If not in cache, call OpenAI
	//	var prompt = string.Format(promptTemplate, string.Join(", ", knownFields), candidateCol);

	//	// Example call to your extension:
	//	// Adjust to your actual method signature or model settings
	//	var rawResponse = AIExtensions.GetOpenAIResponse(prompt).Result;
	//	var classification = rawResponse?.Trim();

	//	// 3) If OpenAI’s classification is not in the known list, set to "Unknown"
	//	if (!knownFields.Any(f => f.Equals(classification, StringComparison.OrdinalIgnoreCase)))
	//	{
	//		classification = "Unknown";
	//	}

	//	// 4) Save to cache
	//	cache[candidateCol] = classification;
	//	SaveOpenAiCache(cache, cachePath);

	//	// 5) Return final
	//	return classification;
	//}

}

public static class NotificationExtensions
{
	public static void SendLinqpadNotification(string title, string description)
	{
		System.Console.Beep();
		System.Windows.Forms.MessageBox.Show(
			text: description,
			caption: title,
			buttons: System.Windows.Forms.MessageBoxButtons.OK,
			icon: System.Windows.Forms.MessageBoxIcon.Information
		);
	}

	public static async Task SendMobileNotification(
	string notificationMessage = "",
	string notificationTitle = "",
	string notificationEndpoint = "",
	string attachmentFilePath = null)
	{
		using var httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri("https://ntfy.sh/");

		// Add title header if provided
		if (!string.IsNullOrEmpty(notificationTitle))
			httpClient.DefaultRequestHeaders.Add("Title", notificationTitle);

		// Get default endpoint if none passed
		if (string.IsNullOrEmpty(notificationEndpoint))
			notificationEndpoint = await Util.GetPasswordAsync("ntfysh.default");

		// If an attachment path is provided AND the file exists, send the file
		if (!string.IsNullOrEmpty(attachmentFilePath) && File.Exists(attachmentFilePath))
		{
			// Read file bytes
			var fileBytes = await File.ReadAllBytesAsync(attachmentFilePath);
			using var fileContent = new ByteArrayContent(fileBytes);

			// Tell ntfy the filename
			fileContent.Headers.Add("Filename", Path.GetFileName(attachmentFilePath));

			// If a message is also present, you can include it in headers or skip it.
			// Option: Put the message in the Title or Message header
			if (!string.IsNullOrEmpty(notificationMessage))
				httpClient.DefaultRequestHeaders.Add("Message", notificationMessage);

			// Use PUT to send the file as the body for an attachment
			await httpClient.PutAsync(notificationEndpoint, fileContent);
		}
		else
		{
			// No attachment — send the text notification as before
			var content = new StringContent(notificationMessage);
			await httpClient.PostAsync(notificationEndpoint, content);
		}
	}

}

public static class InputExtensions
{
	public static string ShowMultiLineInput(string title, string prompt, string defaultText = "")
	{
		// TextBox
		var tb = new TextBox
		{
			Multiline = true,
			ScrollBars = ScrollBars.Both,
			AcceptsReturn = true,
			AcceptsTab = true,
			Text = defaultText,
			Dock = DockStyle.Fill
		};

		// Label for prompt
		var lbl = new Label
		{
			Text = prompt,
			Dock = DockStyle.Top,
			AutoSize = true,
			Padding = new Padding(5)
		};

		// OK button
		var ok = new Button
		{
			Text = "OK",
			DialogResult = DialogResult.OK,
			Dock = DockStyle.Bottom
		};

		// Build form
		using var form = new Form
		{
			Text = title,
			Width = 600,
			Height = 400,
			StartPosition = FormStartPosition.CenterScreen
		};
		form.Controls.Add(tb);
		form.Controls.Add(ok);
		form.Controls.Add(lbl);
		form.AcceptButton = ok;

		return form.ShowDialog() == DialogResult.OK
			? tb.Text
			: null;
	}
}

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

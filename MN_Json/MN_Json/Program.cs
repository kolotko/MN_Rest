using System.Text.Json;
using System.Text.Json.Serialization;
using MN_Json;

Person person = new Person
{
    Name = "Jan Kowalski",
    Age = 30,
    IsStudent = false,
    Date = DateTime.Now
};

// serializacja
string jsonString = JsonSerializer.Serialize(person);
Console.WriteLine(jsonString);
//deserializacja
Person person2 = JsonSerializer.Deserialize<Person>(jsonString);

// zapis do pliku
using FileStream createStream = File.Create("Example.json");
await JsonSerializer.SerializeAsync(createStream, person);

// odczytanie jsona z komentarzami. Technicznie json nie może posiadac komentarzy. Oraz liczb zapisanych jako string
var jsonWithCommentsString = File.ReadAllText("JsonFiles/JsonWithComments.json");

var optionsAlowedCommentAndNumbersAsString = new JsonSerializerOptions
{
    AllowTrailingCommas = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    NumberHandling = JsonNumberHandling.AllowReadingFromString
};

Person person3 = JsonSerializer.Deserialize<Person>(jsonWithCommentsString, optionsAlowedCommentAndNumbersAsString);

// Ignorowanie wielkich i małych liter. Domyślnie uwzględnia wielkość liter
var jsonCaseInsensitive = File.ReadAllText("JsonFiles/JsonCaseInsensitive.json");

var optionsCaseInsensitive = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    NumberHandling = JsonNumberHandling.AllowReadingFromString
};

var optionsCaseInsensitiveAlternative = new JsonSerializerOptions(JsonSerializerDefaults.Web);

Person person4 = JsonSerializer.Deserialize<Person>(jsonCaseInsensitive, optionsCaseInsensitive);
Person person5 = JsonSerializer.Deserialize<Person>(jsonCaseInsensitive, optionsCaseInsensitiveAlternative);

// Jeśli nie może znaleźć pola z obiektu wypełnia go domyślną wartością



//przykład z innymi nazwami pól
var jsonOtherFieldsName = File.ReadAllText("JsonFiles/JsonOtherFieldsName.json");
Person2 person6 = JsonSerializer.Deserialize<Person2>(jsonOtherFieldsName);


//ignorowanie niektórych pól i generowanie jsona z wcięciami, ignoruje kiedy private set i wartośc default
Person3 personIgnore = new Person3
{
    Name = "Jan Kowalski",
    Age = 30,
    IsStudent = default,
    Date = DateTime.Now
};

var optionsWriteIndented = new JsonSerializerOptions
{
    WriteIndented = true,
    IgnoreReadOnlyProperties = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
};

string jsonString222 = JsonSerializer.Serialize(personIgnore, optionsWriteIndented);
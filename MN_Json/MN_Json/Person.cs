using System.Text.Json.Serialization;

namespace MN_Json;

//jeśli będą nadmiarowe property w json to tutaj możesz je pominąć/ lub zabronić ich wystąpieniu
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Skip)]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsStudent { get; set; }
    public DateTime Date { get; set; }
}

// wskaż nazwę pola jeśli inna niż properta
public class Person2
{
    public string Name { get; set; }
    [JsonPropertyName("age2")]
    public int Age { get; set; }
    [JsonPropertyName("isstudent")]
    public bool IsStudent { get; set; }
    public DateTime Date { get; set; }
}

//ignorowanie pól
public class Person3
{
    public string Name { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int Age { get; set; }
    public bool IsStudent { get; set; }
    [JsonIgnore]
    public DateTime Date { get; set; }

    public int AgeReadonly { get; private set; } = 30;
}
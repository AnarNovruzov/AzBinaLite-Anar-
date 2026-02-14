namespace Application.Dtos.City;

public class UpdateCityRequest
{
    public string Name { get; set; }
    public int Id { get; set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

}

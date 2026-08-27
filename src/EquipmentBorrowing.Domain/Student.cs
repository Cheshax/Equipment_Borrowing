namespace EquipmentBorrowing.Domain;

public class Student
{
    public int Id { get; }
    public string name { get; }
    public bool IsAllowedtoBorrow { get; private set; }

    public Student(int id, string name, bool isAllowedtoBorrow = true)
    {
        Id = id;
        this.name = name;
        IsAllowedtoBorrow = isAllowedtoBorrow;
    }

    public void Suspend() => IsAllowedtoBorrow = false;
    public void Reinstate() => IsAllowedtoBorrow = true;

}
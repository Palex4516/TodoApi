namespace TodoApi.Models
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool Done { get; set; }

        public TodoItemDTO() { }
        public TodoItemDTO(TodoItem todoItem) =>
            (Id, Name, Done) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
    }
}

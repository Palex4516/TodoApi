namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public string? Secret { get; set; }

        public TodoItem() { }
        public TodoItem(TodoItemDTO todoItemDTO) =>
            (Id, Name, IsComplete) = (todoItemDTO.Id, todoItemDTO.Name, todoItemDTO.Done);
    }
}

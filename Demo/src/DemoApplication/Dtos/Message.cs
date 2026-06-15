namespace DemoApplication.Dtos;

public record Message(
    DateTime Date,
    string Title,
    string Text,
    string Sender
);

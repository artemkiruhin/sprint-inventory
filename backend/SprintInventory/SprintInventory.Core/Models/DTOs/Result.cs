namespace SprintInventory.Core.Models.DTOs;

public class Result<TResult>
{
    public TResult? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsSuccess { get; init; }

    public static Result<TResult> Success(TResult data)
        => new() { Data = data, IsSuccess = true, ErrorMessage = string.Empty };

    public static  Result<TResult> Failure(string errorMessage)
        => new() { IsSuccess = false, ErrorMessage = errorMessage };
}
namespace LifeManagementApp.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int Priority { get; set; } = 3; // 1–5 (5 = korkein prioriteetti)
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);
    public bool IsDone { get; set; }

    // Vierasavain (Foreign Key) ja navigaatio kategoriaan
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
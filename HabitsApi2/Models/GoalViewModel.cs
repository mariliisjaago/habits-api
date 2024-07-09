﻿using System.Runtime.CompilerServices;

namespace HabitsApi2.Models
{
    public class GoalViewModel
    {
        public int Id { get; set; }
        public int ParentGoalId { get; set; }
        public int FirstChildId { get; set; }
        public int NextSiblingId { get; set; }
        public GoalViewModel? FirstChild { get; set; }
        public GoalViewModel? NextSibling { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsFirstChild { get; set; }
        public bool IsRoot { get; set; }

        public GoalViewModel(int id, int parentGoalId, int firstChildId, int nextSiblingId, string title, string text, bool isCompleted, bool isFirstChild, bool isRoot, GoalViewModel? firstChild, GoalViewModel? nextSibling) 
        {
            Id = id;
            ParentGoalId = parentGoalId;
            FirstChildId = firstChildId;
            NextSiblingId = nextSiblingId;
            Title = title;
            Text = text;
            IsCompleted = isCompleted;
            IsFirstChild = isFirstChild;
            IsRoot = isRoot;
            FirstChild = firstChild;
            NextSibling = nextSibling;
        }

        public GoalViewModel()
        {
            
        }
    }
}

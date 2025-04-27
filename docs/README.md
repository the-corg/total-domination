# TOtal DOmination
![UI demo](TotalDomination.gif)

This is a tiny to-do list app for recurring tasks that I built for myself.

To-do items are sorted and color-coded depending on how long ago they were last done. A tooltip shows detailed statistics.

## Motivation
I needed a to-do list app for tasks that recur regularly, but not every day. I'm happy with my current systems for daily tasks, one-off tasks, and projects. However, for recurring tasks that don't need to be done daily, I didn't have a good system. I didn't like adding them to the daily list because I hated leaving them undone at the end of the day. Adding them to traditional to-do lists with workarounds to account for their recurring nature didn't work either.

That’s why I made an app specifically for recurring tasks, tailored to my own needs. For example, I don't need to add or edit tasks (my list changes very rarely, and it's convenient to store and edit it in a text file), but I do need detailed statistics for each task, including average time between completions and a full history.

## Description
To-do items are added from a pipe-delimited text file ([see example here](RecurrentTodos.txt)). Each task has a "frequency" assigned to it. This is a value that specifies how often I plan to do that task relative to others. For example, I plan to do a task with a frequency of 3 three times more often than a task with a frequency of 1.

The tasks in the list are sorted in descending order by their "urgency" (which is defined as the number of days since they were last done, multiplied by their frequency and the target productivity in tasks per day). This means that tasks with a higher frequency rise to the top faster. If all tasks had the same frequency, the one that hasn't been done for the longest time would be at the top.

In addition, tasks are color-coded based on the same urgency parameter. As a task continues to remain undone, it changes its color from green to yellow, and then to red. For very neglected tasks, I add more and more fire symbols and gradually increase the font size.

## Installation and Running
For the task list, you'll need a pipe-delimited text file with no header row ([see example here](RecurrentTodos.txt)).

```csv
   INT_FREQUENCY|STRING_TITLE
   ```

### Option 1: Download and Run (Windows 7+)
1. Download the latest release from the [GitHub Releases page](https://github.com/the-corg/total-domination/releases).
2. Extract the zipped folder.
3. Run the `.exe` file.

### Option 2: Clone and Build (.NET 8+)
1. Clone this repository.
```sh
   git clone https://github.com/the-corg/total-domination.git
   ```
2. Open the solution in Visual Studio and run the project.

The only dependency is **Microsoft.Extensions.DependencyInjection**.
If NuGet Package Restore doesn't work automatically, you might need to install it manually.
 
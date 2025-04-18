# FlashCards

A console base C# application used to help with learning topics that require recollection, like languages. The user
will set up a set of virtual flash cards with a question on one side and an answer on the other.
Implemented using SQL Server and the Data Transfer Object pattern.

# How to use

TODO

# Requirements

- [ ] Allow users to create stacks of flashcards
- [ ] Two tables, one for stacks and another for flashcards
  - [ ] Tables are linked through foreign keys
- [ ] Every flashcard MUST be a part of a stack
- [ ] If a stack is deleted, the same should happen with the linked flashcards
- [ ] Use DTOs to show the flashcards to the user without the id of the stack it belongs to
- [ ] When showing a stack to the user, flashcard Ids should always start at 1 without any gaps between them
  - [ ] If you have 10 cards and number 5 is deleted show Ids from 1 to 9.
- [ ] After flash card functionality, create a "Study Session" area where the users will study the stacks
  - [ ] All study sessions should be stored with date and score.
  - [ ] The study and stack tables should be linked
    - [ ] If a stack is deleted all its study sessions should also be deleted
- [ ] The project should contain a call to the study table so that users can see all their study sessions
  - [ ] Users should be able to view, but not update, or delete any study session   

## Stretch Goals

- [ ] Report system
  - [ ] Number of sessions per month per stack
  - [ ] Average score per month per stack
- [ ] Sample data generation

# Features

# Challenges

# Lessons Learned

# Areas to Improve

# Resources Used


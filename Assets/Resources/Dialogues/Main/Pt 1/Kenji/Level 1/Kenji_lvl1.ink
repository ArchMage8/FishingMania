EXTERNAL SetVariables()
EXTERNAL SubmitQuest()
EXTERNAL SetActiveQuest()

VAR hasActiveQuest = false
VAR correspondingNPC = false
VAR Success = false

~ SetVariables()   

+{hasActiveQuest && correspondingNPC} -> Submit
+{hasActiveQuest && !correspondingNPC} -> Busy
+{!hasActiveQuest && !correspondingNPC} -> Start

=== Start ===
Hmm?  #speaker: Kenji #trigger: normal
You again?
What now?
    +[How long until the bridge is fixed]
    ->QuestPrompt
    
    +[You seem to be just standing around]
    ->Default_Dialogue
    

=== Busy ===
Hmm?  #speaker: Kenji #trigger: normal
You again?
What now?
    +[How long until the bridge is fixed]
    ->BusyA
    
    +[You seem to be just standing around]
    ->Default_Dialogue
->END

=== BusyA ===
Look didn't someone give you a job?
You should complete that first, and maybe check on me again later...

->END

=== QuestPrompt ===
Shouldn't be much longer, I should able finish it after my lunch break...

Lunch break? #speaker: Iris #trigger: next

Yes, now where is that delivery? #speaker: Kenji #trigger: next

Or did i forget to order it this morning?

You don't even remember? #speaker: Iris #trigger: next

Look I just need to eat something, then I'll get the bridge fixed #speaker: Kenji #trigger: next

    +[What if i get you something?]
    ->QuestStart
    
    +[Fine just fix the bridge soon...]
    ->Default_Ending_2



=== QuestStart ===
Ooh that would be great! #speaker: Kenji #trigger: next
2 grilled fish would be amazing

~ SetActiveQuest()
Dude just wants a free meal... #speaker: Iris #trigger: next

->END



=== Default_Dialogue ===
Its called a break...
Fixing a bridge as 1 person isn't an easy task you know...
    +[Aren't there other workers?]
     ->Default_Ending_1
    
    +[Alrighty then...]
        ->Default_Ending_2


=== Default_Ending_1 ===
 No... Just me
->END

===  Default_Ending_2 ===
Hmm....
->END


=== Submit ===
You got the food? #speaker: Kenji #trigger: submit
    +[Yup Here it is]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Ah, not yet]
    ->Default_Ending_2

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SumbitIncomplete
}

=== SubmitComplete ===
Thank you so much! #speaker: Kenji #trigger: submit
I should be able to get to work now...
Well... After eating of course...
->END
=== SumbitIncomplete ===
Um... Where is it? #speaker: Kenji #trigger: next
->END



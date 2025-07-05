EXTERNAL SetVariables()
EXTERNAL SubmitQuest()
EXTERNAL SetActiveQuest()

VAR hasActiveQuest = false
VAR correspondingNPC = false
VAR Success = false

~ SetVariables()   

{hasActiveQuest && correspondingNPC: 
->Submit

-else:
->Start
}

=== Start ===
Hmm?  #speaker: Kenji #trigger: next
You again?
What now?
    +[How long until the bridge is fixed]
    ->QuestPrompt
    
    +[You seem to be just standing around]
    ->Default_Dialogue
    
    

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
Ooh that would be great!
2 grilled fish would be amazing

Dude just wants a free meal... #speaker: Iris #trigger: next
~ SetActiveQuest()
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
You got the food? #speaker: Kenji #trigger: next
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
Thank you so much!
->END
=== SumbitIncomplete ===
Um... Where is it?
->END



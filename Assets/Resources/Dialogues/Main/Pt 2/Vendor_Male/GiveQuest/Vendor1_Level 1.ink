EXTERNAL SetVariables()
EXTERNAL SubmitQuest()
EXTERNAL SetActiveQuest()

VAR hasActiveQuest = false
VAR correspondingNPC = false
VAR Success = false

~ SetVariables()   

+{hasActiveQuest && correspondingNPC} -> Submit
+{hasActiveQuest && !correspondingNPC} -> Busy
+{!hasActiveQuest} -> Start


=== Start ===
Hey, you there! #speaker: Ren #trigger: next
Are you busy? Willing to help me out? #speaker: Ren
    +[Umm... Sure?]
    ->QuestStart
    
    +[Not right now, sorry.]
    ->Exit1
    
=== QuestStart ===
So I need 2 Lemons! #speaker: Ren
I ran out for my special dish! 
~ SetActiveQuest()

Sure I'll get it for you! #speaker: Iris #trigger: next
->END

=== Busy ===
Hey, you there! #speaker: Ren #trigger: next
Ah you look busy, nevermind then... 
->END

=== Submit ===
You got the goods? #speaker: Ren #trigger: next
    +[Yup here they are!]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Ah not yet...]
    ->Exit1

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SumbitIncomplete
}

=== SubmitComplete ===
Ah great! Thanks a bunch! #speaker: Ren
->END

=== SumbitIncomplete ===
Do my eyes decieve me? #speaker: Ren
Or is my 2 Lemons not there? 
->END

=== Exit1 ===
Ah! Alright then... #speaker: Ren
->END
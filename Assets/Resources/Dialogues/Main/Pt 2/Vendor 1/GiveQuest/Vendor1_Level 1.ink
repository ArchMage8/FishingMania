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
Hey, you there!
Are you busy? Willing to help me out?
    +[Umm... Sure?]
    ->QuestStart
    
    +[Not right now, sorry.]
    ->Exit1
    
=== QuestStart ===
So I need 2 Lemons!
I ran out for my special dish!
~ SetActiveQuest()

Sure I'll get it for you!
->END

=== Busy ===
Hey, you there!
Ah you look busy, nevermind then...
->END

=== Submit ===
You got the goods?
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
Ah great! Thanks a bunch!
->END

=== SumbitIncomplete ===
Do my eyes decieve me?
Or is my 2 Lemons not there?
->END

=== Exit1 ===
Ah! Alright then...
->END
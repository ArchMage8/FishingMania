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
Hey friend!

Who me?

Yes you, willing to help me out?
    +[Sure whats up?]
    ->QuestStart
    +[Umm, not right now]
    ->Exit1
    
=== QuestStart ===
So I've been selling so much food, and I just realised I ran out of some ingredients...

Cam you get me 2 //Insert Item name
~ SetActiveQuest()

Sure, be back soon!
->END

=== Busy ===
Hey friend!
Ah wait, you look like you have something to do, nevermind then...
->END


=== Submit ===
You have it?
    +[Sure do]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Ah... Not yet]
    ->Exit1

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SumbitIncomplete
}

=== SubmitComplete ===
Wow, thanks so much...
->END

=== SumbitIncomplete ===
Ehm, is it hidden in your bag?
Cuz i can't see it...
->END

=== Exit1 ===
Ok then...
->END
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
So... I've considered you request to go fishing

Umm ok...

Buy you have to do something first!
    +[Sure?]
    ->QuestStart
    
    +[Not right now sir...]
    ->Exit1

=== QuestStart ===
I need to make sure you actually know how to fish...
Not like those youngsters who just throw bread and worms into the water and make it all dirty...

What do you need sir?

I need you to catch 3 //insert fish name
That should prove you actually know how to fish...

~ SetActiveQuest()

Are you not just going eat them?

That's besides the point, now go!
->END

=== Busy ===
So... I've considered you request to go fishing

Umm ok...

But finish what the job you have now first!
Can't be holding other people up...

Ok... ok geez
->END

=== Submit ===
You caught them already?
    +[Here take a look]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Not yet...]
    ->Exit2


=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SumbitIncomplete
}

=== SubmitComplete ===
Ah yes these are fine indeed!
I guess you can actually fish!

Well I wouldn't ask to fish if I couldn't...

You have no idea how many people try to fish and just end up falling into the water...

Now leave, I want to enjoy these...

But you said I could fish...

Tomorrow, tomorrow!
Now go!
->END
=== SumbitIncomplete ===
Are you decieving me or something?
Where is it?
->END

=== Exit1 ===
Hmmm?
My offer is not permanent you know!
->END

=== Exit2 ===
You're taking so long...
->END
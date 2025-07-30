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
So... I've considered your request to fish here.

Umm, ok...

But you have to do something first!
    +[Sure?]
    ->QuestStart
    
    +[Not right now sir...]
    ->Exit1

=== QuestStart ===
I need to make sure you actually know how to fish...
Not like those youngsters who just throw bread and worms into the water and make it all dirty...

What do you need sir?

I need you to catch 3 Sea Bass.
That should prove you actually know how to fish...

~ SetActiveQuest()

Are you not just going to eat them?

That's besides the point! Now go!
->END

=== Busy ===
So... I've considered your request to fish here.

Umm, ok...

But finish the job you currently have first!
Can't be holding other people up!

Ok... Geez.
->END

=== Submit ===
Have you caught them?
    +[Here take a look!]
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
Ah yes, these are fine indeed!
I guess you can actually fish!

Well, I wouldn't ask to fish if I couldn't...

You have no idea how many people try to fish and just end up falling into the water...

Now leave, I want to enjoy these...

But you said I could fish...

Tomorrow, tomorrow!
Now go!
->END
=== SumbitIncomplete ===
Are you trying to deceive me?
Where is the 3 Sea Bass?
->END

=== Exit1 ===
Hmmm?
My offer is not permanent you know!
->END

=== Exit2 ===
You're really slow for someone your age...
->END
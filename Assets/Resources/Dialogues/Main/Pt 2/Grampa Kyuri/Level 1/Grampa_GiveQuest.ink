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
So... I've considered your request to fish here.  #speaker: Grandpa Kyuri #trigger: Start

Umm, ok... #speaker: Iris #trigger: next

But you have to do something first! #speaker: Grandpa Kyuri #trigger: next
    +[Sure?]
    ->QuestStart
    
    +[Not right now sir...]
    ->Exit1

=== QuestStart ===
I need to make sure you actually know how to fish... #speaker: Grandpa Kyuri #trigger: next
Not like those youngsters who just throw bread and worms into the water and make it all dirty...

What do you need sir? #speaker: Iris #trigger: next

I need you to catch 3 Sea Bass. #speaker: Grandpa Kyuri #trigger: next
That should prove you actually know how to fish...

~ SetActiveQuest()

Are you not just going to eat them? #speaker: Iris #trigger: next

That's besides the point! Now go! #speaker: Grandpa Kyuri #trigger: next
->END

=== Busy ===
So... I've considered your request to fish here. #speaker: Grandpa Kyuri #trigger: BusyStart

Umm, ok... #speaker: Iris #trigger: next

But finish the job you currently have first! #speaker: Grandpa Kyuri #trigger: next
Can't be holding other people up! 

Ok... Geez. #speaker: Iris #trigger: next
->END

=== Submit ===
Have you caught them? #speaker: Grandpa Kyuri #trigger: SubmitStart
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
Ah yes, these are fine indeed! #speaker: Grandpa Kyuri #trigger: Complete
I guess you can actually fish!

Well, I wouldn't ask to fish if I couldn't... #speaker: Iris #trigger: next

You have no idea how many people try to fish and just end up falling into the water... #speaker: Grandpa Kyuri #trigger: next

Now leave, I want to enjoy these...

But you said I could fish... #speaker: Iris #trigger: next

Tomorrow, tomorrow! #speaker: Grandpa Kyuri #trigger: next
Now go!
->END
=== SumbitIncomplete ===
Are you trying to deceive me? #speaker: Grandpa Kyuri #trigger: Incomplete
Where is the 3 Sea Bass?
->END

=== Exit1 ===
Hmmm? #speaker: Grandpa Kyuri #trigger: Exit1
My offer is not permanent you know!
->END

=== Exit2 ===
You're really slow for someone your age... #speaker: Grandpa Kyuri #trigger: Exit2
->END
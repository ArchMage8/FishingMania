EXTERNAL SetVariables()
EXTERNAL SubmitQuest()

VAR hasActiveQuest = false
VAR correspondingNPC = false
VAR Success = false

// ~ SetVariables()
// ~ SubmitQuest()

Hello there!
~ SetVariables()
-> 1A

=== 1A ===
{ hasActiveQuest == false:
    -> 2B
- else:
    { correspondingNPC == false:
        -> 2A
    - else:
        -> 2C
    }
}


=== 2A === // Is busy
Looks like your busy, nevermind then
-> END

=== 2B === // Not busy
I really need some salt, can you help me?
    +[Sure, let me get some]
       -> 3A
    
    +[Ah, im a bit busy right now]
       -> 3B

=== 2C === // Is busy and want so sumbit
    Do you have the salt?
    +[Here you go!]
        ~ SubmitQuest()
        -> Process2
    
    +[I don't have it yet]
        Ah, alright then!
        -> 3E


=== Process2 ===

{Success == true:
    -> 3C
- else:
    -> 3D
}



=== 3A === //Setting the active quest
Thank you!
//Missing the function needed to set the active quest :v
-> END
    
=== 3B === // Reject Quest
 Nevermind then...
-> END

=== 3C === // quest complete
Thank you so much!
//Missing the function to complete the quest system-wise
-> END

=== 3D === // quest fail
Um.. Where is it?
    -> END

=== 3E === // quest pending
 Ah, alright then!
        -> END











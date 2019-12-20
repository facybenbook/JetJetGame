import bpy
from bpy import context

import builtins as __builtin__

def console_print(*args, **kwargs):
    for window in bpy.context.window_manager.windows:
        for a in window.screen.areas:
            if a.type == 'CONSOLE':
                c = {}
                c['area'] = a
                c['space_data'] = a.spaces.active
                c['region'] = a.regions[-1]
                c['window'] = window
                c['screen'] = window.screen
                s = " ".join([str(arg) for arg in args])
                for line in s.split("\n"):
                    bpy.ops.console.scrollback_append(c, text=line)

def print2(*args, **kwargs):
    """Console print() function."""

    console_print(*args, **kwargs) # to py consoles
    __builtin__.print(*args, **kwargs) # to system console

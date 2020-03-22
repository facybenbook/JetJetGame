
bl_info = {
    "name" : "HappyRobot Fantasmagoria",
    "author" : "HappyRobot",
    "description" : "Add On!",
    "blender" : (2, 80, 0),
    "location" : "View3D",
    "warning" : "",
    "category" : "Generic"
}

#https://blender.stackexchange.com/questions/158287/how-can-i-create-an-advanced-addon-with-a-gui-and-multiple-settings-for-blender

import bpy

from . Interface import Interface_PT_Panel
from . OriginToSelected import OriginToSelected_OT_Operator
from . Voxelize import OBJECT_OT_Voxelize
from . MakeWire import OBJECT_OT_MakeWire, OBJECT_OT_MakeWire2, OBJECT_OT_MakeWire3, OBJECT_OT_MakeWire4, OBJECT_OT_MakeWire5, OBJECT_OT_MakeWire6, OBJECT_OT_MakeWire7, OBJECT_OT_MakeWire8
from . FBXExport import OBJECT_OT_FBXExport
from . Rename import OBJECT_OT_Rename

classes = (Interface_PT_Panel, OriginToSelected_OT_Operator, OBJECT_OT_Voxelize, OBJECT_OT_MakeWire, OBJECT_OT_MakeWire2, OBJECT_OT_MakeWire3, OBJECT_OT_MakeWire4, OBJECT_OT_MakeWire5, OBJECT_OT_MakeWire6, OBJECT_OT_MakeWire7, OBJECT_OT_MakeWire8, OBJECT_OT_FBXExport, OBJECT_OT_Rename)

register, unregister = bpy.utils.register_classes_factory(classes)

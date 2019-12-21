import bpy
from . Print import print2

class OriginToSelected_OT_Operator(bpy.types.Operator):
    """Set the mesh's origin to the centroid of selected vertex, edge or face"""
    bl_idname = "view3d.origin_to_selection"
    bl_label = "Set Origin to Selection"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.edit_object and context.edit_object.type == 'MESH'

    def execute(self, context):


        bpy.ops.view3d.snap_cursor_to_selected()
        bpy.ops.object.editmode_toggle()
        bpy.ops.object.origin_set(type='ORIGIN_CURSOR')
        bpy.ops.object.editmode_toggle()

        return {'FINISHED'}

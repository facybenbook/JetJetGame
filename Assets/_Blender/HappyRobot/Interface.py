import bpy
from . Print import print2

class Interface_PT_Panel(bpy.types.Panel):
    bl_idname = "Interface_PT_Panel"
    bl_label = "HappyRobot Studio"
    bl_category = "HappyRobot"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"

    def draw(self, context):
        layout = self.layout

        scene = context.scene

        layout.label(text= "Set Origin:")

        row = layout.row()
        row.operator("view3d.origin_to_selection", text = "Set Origin to Selection", icon='WORLD_DATA')

        layout.label(text= "Voxelize:")

        row = layout.row()
        row.operator("object.voxelize", text = "Voxelize", icon='WORLD_DATA')

        layout.label(text= "Wireframe:")

        row = layout.row()
        row.operator("object.make_a_wireframe", text = "Make Wireframe", icon='WORLD_DATA')

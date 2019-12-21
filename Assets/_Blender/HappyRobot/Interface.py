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
        row.operator("object.make_a_wireframe", text = "Type 1", icon='WORLD_DATA')

        row = layout.row()
        row.operator("object.make_a_wireframe2", text = "Type 2", icon='WORLD_DATA')

        row = layout.row()
        row.operator("object.make_a_wireframe3", text = "Type 3", icon='WORLD_DATA')

        row = layout.row()
        row.operator("object.make_a_wireframe4", text = "Type 4", icon='WORLD_DATA')

        row = layout.row()
        row.operator("object.make_a_wireframe5", text = "Type 5", icon='WORLD_DATA')

        row = layout.row()
        row.operator("object.make_a_wireframe6", text = "Type 6", icon='WORLD_DATA')

        row = layout.row()
        row.operator("object.make_a_wireframe7", text = "Type 7", icon='WORLD_DATA')

        row = layout.row()
        row.operator("object.make_a_wireframe8", text = "Type 8", icon='WORLD_DATA')

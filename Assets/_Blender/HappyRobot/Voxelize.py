import bpy
from . Print import print2

from bpy.types import(
    AddonPreferences,
    Operator,
    Panel,
    PropertyGroup,
)

from bpy.props import(IntProperty)

class OBJECT_OT_Voxelize(Operator):
    bl_label = "Voxelize"
    bl_idname = "object.voxelize"
    bl_description = "Converts model to Voxel mesh"
    bl_space_type = 'VIEW3D'
    bl_region_type = 'UI'
    bl_options = {'REGISTER', 'UNDO'}

    voxelizeResolution: bpy.props.IntProperty(
        name = "Voxelize Resolution",
        default = 6,
        min = 1,
        max = 15,
        description = "Octree depth used in the Remesh Modifier",
    )

    @classmethod
    def poll(cls, context):
        #return context.object.select_get() and context.object.type == 'MESH' # or context.object.type == 'CURVE'
        return context.object.select_get() and context.mode in {'OBJECT'}
        # ob = context.edit_object
        # return ob and ob.type == 'MESH'

    def invoke(self, context, event):
        return context.window_manager.invoke_props_dialog(self)

    def execute(self, context):
        #start to copy object
        print2("Voxelize !")
        sourceName = bpy.context.object.name
        source = bpy.data.objects[sourceName]

        bpy.ops.object.duplicate_move(OBJECT_OT_duplicate={"linked":False, "mode":'TRANSLATION'})
        bpy.context.object.name = sourceName + '_Voxel'
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=False)
        bpy.ops.object.convert(target='MESH')

        #source.hide_render = True
        #source.hide_viewport = True
        targetName = bpy.context.object.name
        target = bpy.data.objects[targetName]
        #start to voxelize
        bpy.ops.object.modifier_add(type='REMESH')
        bpy.context.object.modifiers["Remesh"].mode = 'BLOCKS'
        bpy.context.object.modifiers["Remesh"].octree_depth = self.voxelizeResolution
        bpy.context.object.modifiers["Remesh"].use_remove_disconnected = False
        bpy.ops.object.modifier_apply(apply_as='DATA', modifier="Remesh")


        #transfer UV Data
        bpy.ops.object.modifier_remove(modifier="DataTransfer")
        bpy.ops.object.modifier_add(type='DATA_TRANSFER')
        bpy.context.object.modifiers["DataTransfer"].use_loop_data = True
        bpy.context.object.modifiers["DataTransfer"].data_types_loops = {'UV'}
        bpy.context.object.modifiers["DataTransfer"].loop_mapping = 'POLYINTERP_NEAREST'
        bpy.context.object.modifiers["DataTransfer"].object = source
        bpy.ops.object.datalayout_transfer(modifier="DataTransfer")
        bpy.ops.object.modifier_apply(apply_as='DATA', modifier="DataTransfer")

        #reduce to face texture
        bpy.ops.object.editmode_toggle()
        bpy.ops.mesh.select_mode(type='FACE')

        bpy.context.area.ui_type = 'UV'
        bpy.context.scene.tool_settings.use_uv_select_sync = False
        bpy.context.space_data.uv_editor.sticky_select_mode = 'DISABLED'
        bpy.context.scene.tool_settings.uv_select_mode = 'FACE'
        bpy.context.space_data.pivot_point = 'INDIVIDUAL_ORIGINS'
        #bpy.ops.mesh.select_random(seed=1)



        count = 0
        while count < 100:
            bpy.ops.mesh.select_random(percent=count+1, seed=count)
            bpy.ops.uv.select_all(action='SELECT')
            bpy.ops.transform.resize(value=(0.001,0.001,0.001))
            bpy.ops.mesh.hide(unselected=False)
            count+=1

        #rever to previous context
        bpy.context.area.ui_type = 'VIEW_3D'
        bpy.ops.mesh.reveal()
        bpy.ops.object.editmode_toggle()
        return {'FINISHED'}

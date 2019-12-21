import bpy
import os
from . Print import print2

class OBJECT_OT_FBXExport(bpy.types.Operator):
    """Batch FBX Exporter"""
    bl_idname = "object.fbx_export"
    bl_label = "Batch FBX Export selected objects to same folder as blend file"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}


    def execute(self, context):
        # export to blend file location
        basedir = os.path.dirname(bpy.data.filepath)

        if not basedir:
            raise Exception("Blend file is not saved")

        view_layer = bpy.context.view_layer

        obj_active = view_layer.objects.active
        selection = bpy.context.selected_objects

        bpy.ops.object.select_all(action='DESELECT')

        for obj in selection:

            obj.select_set(True)

            # some exporters only use the active object
            view_layer.objects.active = obj

            name = bpy.path.clean_name(obj.name)
            fn = os.path.join(basedir, name)

            bpy.ops.export_scene.fbx(filepath=fn + ".fbx", use_selection=True)

            # Can be used for multiple formats
            # bpy.ops.export_scene.x3d(filepath=fn + ".x3d", use_selection=True)

            obj.select_set(False)

            print2("written:", fn)


        view_layer.objects.active = obj_active

        for obj in selection:
            obj.select_set(True)

        return { 'FINISHED' }

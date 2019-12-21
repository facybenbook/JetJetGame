import bpy
import sys

from . Print import print2
from . WireSkin import WireSkin

def wire_to_skin(** kwargs):
    print2("wire_to_skin !")
    sourceName = bpy.context.object.name
    source = bpy.data.objects[sourceName]

    bpy.ops.object.duplicate_move(OBJECT_OT_duplicate={"linked":False, "mode":'TRANSLATION'})
    bpy.context.object.name = sourceName + '_Wire'
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=False)
    bpy.ops.object.convert(target='MESH')
    #source.hide_render = True
    #source.hide_viewport = True
    targetName = bpy.context.object.name
    target = bpy.data.objects[targetName]
    # source = bpy.data.objects[in_name]
    # target = bpy.data.objects[out_name]
    output_materials = target.data.materials

    wire_skin = \
        WireSkin(source.data, ** kwargs)

    me = wire_skin.create_mesh()
    for material in output_materials:
        me.materials.append(material)
    target.data = me

def layer1():
    options = {
        'width': 10.15,
        'height': 10.1,
        'inside_radius': 0,
        'outside_radius': 0,
        'dist': 0
    }
    wire_to_skin(** options)

def layer2():
    options = {
        'width': 0.1,
        'height': 0.2,
        'inside_radius': 0.1,
        'outside_radius': 0.1,
        'dist': 0.2,
        'crease': 1.0
    }
    wire_to_skin(** options)

def layer3():
    options = {
        'width': 0.1,
        'height': 0.2,
        'inside_radius': 0.1,
        'outside_radius': 0.8,
        'dist': 0.2,
    }
    wire_to_skin(** options)

def layer4():
    options = {
        'width': 0.5,
        'height': 0.2,
        'inside_radius': 0.1,
        'outside_radius': 0.1,
        'dist': 0.8,
    }
    wire_to_skin(** options)

    options = {
        'width': 0.3,
        'height': 0.3,
        'inside_radius': 0.1,
        'outside_radius': 0.1,
        'dist': 0.8,
    }
    wire_to_skin(** options)

    options = {
        'width': 0.1,
        'height': 0.4,
        'inside_radius': 0.1,
        'outside_radius': 0.1,
        'dist': 0.8,
    }
    wire_to_skin(** options)

def layer5():
    options = {
        'width': 0.5,
        'height': 0.3,
        'inside_radius': 0.1,
        'outside_radius': 0.1,
        'dist': 1.0,
        'crease': 1.0
    }
    wire_to_skin(** options)

    # This one is on layer 15# It is subtracted from previous object
    options = {
        'width': 0.3,
        'height': 0.3,
        'inside_radius': 0.1,
        'outside_radius': 0.1,
        'dist': 0.8,
        'crease': 1.0,
        'displace': 0.15
    }
    wire_to_skin(** options)

    options = {
        'width': 0.05,
        'height': 0.3,
        'inside_radius': 0.1,
        'outside_radius': 0.1,
        'dist': 0.8,
        'crease': 1.0,
    }
    wire_to_skin(** options)

def layer6():
    options = {
        'width': 0.07,
        'height': 0.05,
        'inside_radius': 0.04,
        'outside_radius': 0.02,
        'dist': 0.025
    }
    wire_to_skin(** options)

def layer7():
    options = {
        'width': 0.3,
        'height': 0.3,
        'inside_radius': 0.2,
        'outside_radius': 0.1,
        'dist': 0.3,
        'proportional_scale': True
    }
    wire_to_skin(** options)

def layer8():
    options = {
        'width': 0.15,
        'height': 0.1,
        'inside_radius': 0.3,
        'outside_radius': 0.8,
        'dist': 0.4
    }
    wire_to_skin(** options)
    options['edges_without_poles'] = True
    wire_to_skin(** options)


class OBJECT_OT_MakeWire(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe"
    bl_label = "Wireframe1"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer1()
        return {'FINISHED'}


class OBJECT_OT_MakeWire2(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe2"
    bl_label = "Wireframe2"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer2()
        return {'FINISHED'}

class OBJECT_OT_MakeWire3(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe3"
    bl_label = "Wireframe3"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer3()
        return {'FINISHED'}

class OBJECT_OT_MakeWire4(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe4"
    bl_label = "Wireframe4"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer4()
        return {'FINISHED'}

class OBJECT_OT_MakeWire5(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe5"
    bl_label = "Wireframe5"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer1()
        return {'FINISHED'}


class OBJECT_OT_MakeWire6(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe6"
    bl_label = "Wireframe6"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer2()
        return {'FINISHED'}

class OBJECT_OT_MakeWire7(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe7"
    bl_label = "Wireframe7"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer3()
        return {'FINISHED'}

class OBJECT_OT_MakeWire8(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe8"
    bl_label = "Wireframe8"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object and context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        layer4()
        return {'FINISHED'}

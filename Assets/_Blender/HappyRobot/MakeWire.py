import bpy
import sys

from . Print import print2
from . WireSkin import WireSkin


class OBJECT_OT_MakeWire(bpy.types.Operator):
    """Make a wireframe"""
    bl_idname = "object.make_a_wireframe"
    bl_label = "Make a wireframe"
    bl_options = {'REGISTER', 'UNDO'}

    @classmethod
    def poll(cls, context):
        return context.object.select_get() and context.mode in {'OBJECT'}

    def execute(self, context):
        # A bunch of layers
        print2("OBJECT_OT_MakeWire")

        return {'FINISHED'}


    def wire_to_skin(in_name, out_name, ** kwargs):
        input_object = bpy.data.objects[in_name]
        output_object = bpy.data.objects[out_name]
        output_materials = output_object.data.materials

        wire_skin = \
            WireSkin(input_object.data, ** kwargs)

        me = wire_skin.create_mesh()
        for material in output_materials:
            me.materials.append(material)
        output_object.data = me

    def layer1():
        options = {
            'width': 0.15,
            'height': 0.1,
            'inside_radius': 0.3,
            'outside_radius': 0.8,
            'dist': 0.4
        }
        wire_to_skin("Wire1", "WireSkin1", ** options)

    def layer2():
        options = {
            'width': 0.1,
            'height': 0.2,
            'inside_radius': 0.1,
            'outside_radius': 0.1,
            'dist': 0.2,
            'crease': 1.0
        }
        wire_to_skin("Wire2", "WireSkin2", ** options)

    def layer3():
        options = {
            'width': 0.1,
            'height': 0.2,
            'inside_radius': 0.1,
            'outside_radius': 0.8,
            'dist': 0.2,
        }
        wire_to_skin("Wire3", "WireSkin3", ** options)

    def layer4():
        options = {
            'width': 0.5,
            'height': 0.2,
            'inside_radius': 0.1,
            'outside_radius': 0.1,
            'dist': 0.8,
        }
        wire_to_skin("Wire4", "WireSkin4", ** options)

        options = {
            'width': 0.3,
            'height': 0.3,
            'inside_radius': 0.1,
            'outside_radius': 0.1,
            'dist': 0.8,
        }
        wire_to_skin("Wire4", "WireSkin4a", ** options)

        options = {
            'width': 0.1,
            'height': 0.4,
            'inside_radius': 0.1,
            'outside_radius': 0.1,
            'dist': 0.8,
        }
        wire_to_skin("Wire4", "WireSkin4b", ** options)

    def layer5():
        options = {
            'width': 0.5,
            'height': 0.3,
            'inside_radius': 0.1,
            'outside_radius': 0.1,
            'dist': 1.0,
            'crease': 1.0
        }
        wire_to_skin("Wire5", "WireSkin5", ** options)

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
        wire_to_skin("Wire5", "WireSkin5a", ** options)

        options = {
            'width': 0.05,
            'height': 0.3,
            'inside_radius': 0.1,
            'outside_radius': 0.1,
            'dist': 0.8,
            'crease': 1.0,
        }
        wire_to_skin("Wire5", "WireSkin5b", ** options)

    def layer6():
        options = {
            'width': 0.07,
            'height': 0.05,
            'inside_radius': 0.04,
            'outside_radius': 0.02,
            'dist': 0.025
        }
        wire_to_skin("Wire6", "WireSkin6", ** options)

    def layer7():
        options = {
            'width': 0.3,
            'height': 0.3,
            'inside_radius': 0.2,
            'outside_radius': 0.1,
            'dist': 0.3,
            'proportional_scale': True
        }
        wire_to_skin("Wire7", "WireSkin7", ** options)

    def layer8():
        options = {
            'width': 0.15,
            'height': 0.1,
            'inside_radius': 0.3,
            'outside_radius': 0.8,
            'dist': 0.4
        }
        wire_to_skin("Wire8", "WireSkin8", ** options)
        options['edges_without_poles'] = True
        wire_to_skin("Wire8", "WireSkin8a", ** options)

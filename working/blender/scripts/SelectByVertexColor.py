    #----------------------------------------------------------
# File invoke.py
# from API documentation
#----------------------------------------------------------

op_name = 'object.select_by_vertex_color'
op_label = 'Select By Vertex Color'

bl_info = {
    'name': op_label,
    'category': 'Mesh',
}

import bpy
import bmesh
import os
from bpy.props import *
from bpy.types import (Panel, Operator, PropertyGroup)


def rgb_to_hex(rgb):
    r = int(255 * rgb.r)
    g = int(255 * rgb.g)
    b = int(255 * rgb.b)
    value = (r << 16) | (g << 8) | b
    return value

class SelectByVertexColorOperator(bpy.types.Operator):
    bl_label = op_label
    bl_idname = op_name

    def execute(self, context):
        print()
        print(self.bl_idname)

        obj = context.active_object
        mesh = obj.data

        my_value = context.scene.my_value

        # print(obj.mode)

        if obj.mode != 'EDIT':
            bpy.ops.object.mode_set(mode='EDIT')

        bm = bmesh.from_edit_mesh(mesh)   # fill it in from a Mesh
        color_layer = None
        try:
            color_layer = bm.loops.layers.color['Col']
        except:
            color_layer = bm.loops.layers.color.new('Col')

        for face in bm.faces:
            for loop in face.loops:
                value1 = rgb_to_hex(my_value)
                value2 = rgb_to_hex(loop[color_layer])
                if value1 == value2:
                    face.select = True

        obj.data.update()

        # Finish up, write the bmesh back to the mesh
        # if obj.mode == 'EDIT':
        #     bmesh.update_edit_mesh(mesh)
        # else:            
        #     bm.to_mesh(mesh)
        #     bm.free()

        print('Done')
        return {'FINISHED'}

    def invoke(self, context, event):
        return self.execute(context)

class SelectByVertexColorPanel(bpy.types.Panel):
    bl_label = op_label
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'TOOLS'

    def draw(self, context):
        scene = context.scene
        layout = self.layout
        layout.prop(context.scene, "my_value")
        layout.operator(op_name, text="Select")

def register():
    bpy.types.Scene.my_value = FloatVectorProperty(
        name="",
        subtype='COLOR_GAMMA',
        description="Sample color",
        )

    bpy.utils.register_class(SelectByVertexColorOperator)
    bpy.utils.register_module(__name__)

def unregister():
    bpy.utils.unregister_module(__name__)

if __name__ == '__main__':
    register()

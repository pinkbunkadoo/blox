#----------------------------------------------------------
# File invoke.py
# from API documentation
#----------------------------------------------------------

bl_info = {
    'name': 'Instant FBX',
    'category': 'Import-Export',
}

import bpy
import os
from bpy.props import *
from bpy.types import (Panel, Operator, PropertyGroup)

class InstantFbxOperator(bpy.types.Operator):
    bl_idname = 'export.instant_fbx'
    bl_label = 'Export FBX'

    #x = bpy.props.IntProperty()
    #y = bpy.props.IntProperty()

    def execute(self, context):
        # rather then printing, use the report function,
        # this way the message appears in the header,

        ob = context.active_object
        my_tool = context.scene.my_tool
        filepath = bpy.path.abspath(my_tool.my_string)

        # filepath = bpy.data.filepath
        # directory = os.path.dirname(filepath)
        # newfilename = os.path.join(directory, ob.name + '.fbx')
        newfilename = filepath + ob.name + '.fbx'

        print('Exporting...')

        bpy.ops.export_scene.fbx(
            filepath=str(newfilename),
            use_selection=True,
            apply_unit_scale=False,
            # axis_forward='Z',
            # axis_up='Y',
            bake_space_transform=True
            # object_types={'MESH'}
            )

        # self.report({'INFO'}, 'Exported FBX')

        print(newfilename)

        return {'FINISHED'}

    def invoke(self, context, event):
        #self.x = event.mouse_x
        #self.y = event.mouse_y
        return self.execute(context)

#
#    Panel in tools region
#
class InstantFbxPanel(bpy.types.Panel):
    bl_label = 'Instant FBX'
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'TOOLS'
    bl_category = 'Export'
    # bl_category = 'Export'
    # bl_category = 'Tools'

    def draw(self, context):
        scene = context.scene
        layout = self.layout
        mytool = scene.my_tool
        layout.label('Save Path:')
        layout.prop(mytool, 'my_string')
        layout.operator('export.instant_fbx')

class MySettings(PropertyGroup):
    my_string = StringProperty(name='', description = 'Path', default = '//', subtype='DIR_PATH')

#
#	Registration
#   Not really necessary to register the class, because this happens
#   automatically when the module is registered. OTOH, it does not hurt either.

def register():
    bpy.utils.register_class(InstantFbxOperator)
    bpy.utils.register_module(__name__)
    bpy.types.Scene.my_tool = PointerProperty(type=MySettings)

def unregister():
    bpy.utils.unregister_module(__name__)
    del bpy.types.Scene.my_tool

if __name__ == '__main__':
    register()

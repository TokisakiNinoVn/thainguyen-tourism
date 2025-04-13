//upload.api.js
import instance from '@/apis/axiosConfig';

const uploadSingleFileApi = async (formData) => instance.post('/api/admin/place-media/upload-single', 
    formData, {
    headers: {
        'Content-Type': 'multipart/form-data',
    },
});
const uploadMultipleApi = async (formData) => instance.post('/api/admin/place-media/upload-multiple', 
    formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
});

export {
    uploadSingleFileApi,
    uploadMultipleApi
};

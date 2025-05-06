import instance from '@/services/axiosConfig';

// Get all chat messages
const getAllBlogApi = async () => instance.get('api/web/blog/');
const postCreatBlogApi = async (data) => instance.post('api/web/blog/create', data);

const uploadSingleFileApi = async (formData) => instance.post('api/web/blog/upload-file', 
    formData, {
    headers: {
        'Content-Type': 'multipart/form-data',
    },
});

export {    
    getAllBlogApi,
    postCreatBlogApi,
    uploadSingleFileApi
};
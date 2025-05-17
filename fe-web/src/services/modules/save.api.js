import instance from '@/services/axiosConfig';

const getSavePlaceApi = async () => instance.get('api/web/save/saved-places');

const postSavePlaceApi = async (data) => instance.post('api/web/save/save', data);
// unsave place
const deleteUnSavePlaceApi = async (id) => instance.delete(`api/web/save/unsave/${id}`);


export {    
    postSavePlaceApi,
    getSavePlaceApi,
    deleteUnSavePlaceApi
};
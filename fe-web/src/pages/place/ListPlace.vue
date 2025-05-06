<template>
    <NavbarComponentV1 />

    <div class="container mx-auto px-4 py-6">
    <h2 class="text-2xl font-bold mb-6">Danh sách địa điểm</h2>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
        v-for="place in listPlace"
        :key="place.id"
        class="bg-white rounded-xl shadow p-4 flex flex-col"
        >
        <img
        v-if="Number.isInteger(place.thumbnail)"
            :src="instance.defaults.baseURL + place.imageUrl"
            alt="Thumbnail"
            class="h-48 w-full object-cover rounded-md mb-4"
        />
        <img
        v-else
            src="https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg"
            alt="Thumbnail"
            class="h-48 w-full object-cover rounded-md mb-4"
        />
        <h3 class="text-xl font-semibold mb-2">{{ place.name }}</h3>
        <p class="text-gray-700 mb-4 line-clamp-3" v-html="getShortDescription(place.description)"></p>
        <!-- <button
            class="mt-auto bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition"
            @click="viewDetail(place.id)"
        >
            Xem chi tiết
        </button> -->
        <router-link 
            class="mt-auto bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition"
            :to="`/places/${place.id}`"
        >
            <i class="fas fa-plus"></i>
            Xem chi tiết
        </router-link>
        </div>
    </div>
    </div>

    <FooterComponent />
</template>
<script setup>
import { onMounted, ref } from 'vue';
// import { useRouter } from 'vue-router';
import NavbarComponentV1 from '@/components/NavbarComponentV1.vue';
import FooterComponent from '@/components/FooterComponent.vue';
import { getListPlaceApi } from '@/services/modules/place.api';
import instance from '@/services/axiosConfig';

const listPlace = ref([]);
// const router = useRouter();

const getListPlace = async () => {
    try {
        const response = await getListPlaceApi();
        listPlace.value = response.data.data;
    } catch (error) {
    console.error('Error fetching list of places:', error);
    }
};

// const viewDetail = (id) => {
//     router.push({ name: 'PlaceDetail', params: { id } });
// };

// Cắt mô tả chỉ hiển thị khoảng 3 dòng
const getShortDescription = (htmlDescription) => {
    const tempEl = document.createElement('div');
    tempEl.innerHTML = htmlDescription;
    const text = tempEl.textContent || tempEl.innerText || '';
    return text.length > 200 ? text.slice(0, 200) + '...' : text;
};

onMounted(() => {
    getListPlace();
});
</script>


<style>
</style>

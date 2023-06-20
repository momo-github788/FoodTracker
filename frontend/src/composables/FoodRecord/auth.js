import AuthService from '@/services/AuthService'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'



export default function useAuth() {

    const router = useRouter();

    const errors = ref([]) //array of strings


    const register = async (request) => {
        await AuthService.register(request)
            .then(res => {
                console.log("res: "+ res)
            })
            .catch(err => {
                console.log("err: ", err.response)
            })
    }


    return {
        register
    }
}
import { useContext } from 'react'
import { store } from '../components/store'

export const useStore = () => {
    const { state, dispatch } = useContext(store)
    return { state, dispatch }
}
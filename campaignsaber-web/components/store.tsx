import React, {
  useReducer,
  createContext,
  Reducer,
  FunctionComponent,
} from 'react'

export type User = {
  id: string
  role: Role
  profile: Profile
}

export enum Role {
  None = 0,
  Admin = 1,
}

export type Profile = {
  id: string
  username: string
  discriminator: string
  avatar: string
};

export const defaultUser: LoginState = {
  token: null,
  user: null,
  expiration: 0,
}

export type LoginState = {
  user: User
  token: string
  expiration: number
}

interface IContext {
  state: LoginState
  dispatch: React.Dispatch<Action>
}

const initialState: LoginState = { user: null, token: null, expiration: 0 }

// @ts-ignore
export const store = createContext<IContext>({ state: initialState })

type Action =
  | { type: 'setLoginData'; value: LoginState }
  | { type: 'clearLoginData'; value: void }

export const Provider: FunctionComponent = ({ children }) => {
  const [state, dispatch] = useReducer<Reducer<LoginState, Action>>(
    (prevState, action) => {
      switch (action.type) {
        case 'setLoginData':
          prevState = {
            user: action.value.user,
            token: action.value.token,
            expiration: action.value.expiration,
          };
          return { ...prevState }
        case 'clearLoginData':
          prevState = {
            user: null,
            token: null,
            expiration: 0,
          };
          return { ...prevState }
        default:
          throw new Error('Invalid Action');
      }
    },
    initialState
  )

  return (
    <store.Provider value={{ state, dispatch }}>{children}</store.Provider>
  )
}
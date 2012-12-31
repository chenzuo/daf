﻿var chars = {
    a: "M34.999,64.015c3.671-0.46,6.296-1.038,7.877-1.73c2.835-1.199,4.253-3.068,4.253-5.605c0-3.09-1.088-5.225-3.262-6.401c-2.175-1.177-5.367-1.765-9.575-1.765c-4.724,0-8.068,1.154-10.032,3.46c-1.403,1.708-2.339,4.014-2.806,6.92H2.422c0.416-6.597,2.27-12.018,5.56-16.263c5.236-6.644,14.225-9.965,26.968-9.965c8.294,0,15.661,1.638,22.103,4.914c6.44,3.276,9.661,9.458,9.661,18.547v34.603c0,2.399,0.045,5.306,0.139,8.72c0.138,2.584,0.53,4.337,1.176,5.26c0.646,0.923,1.614,1.685,2.907,2.284v2.907H49.482c-0.6-1.522-1.016-2.952-1.246-4.291c-0.231-1.338-0.416-2.86-0.554-4.568c-2.738,2.953-5.895,5.467-9.467,7.543c-4.269,2.445-9.096,3.668-14.479,3.668c-6.869,0-12.541-1.95-17.019-5.848C2.238,102.505,0,96.98,0,89.829c0-9.273,3.602-15.987,10.806-20.139c3.951-2.26,9.761-3.875,17.43-4.845L34.999,64.015z M47.059,73.219c-1.263,0.785-2.538,1.418-3.824,1.903c-1.287,0.484-3.051,0.934-5.296,1.349l-4.49,0.831c-4.208,0.739-7.23,1.638-9.066,2.699c-3.106,1.799-4.659,4.591-4.659,8.374c0,3.369,0.948,5.803,2.846,7.301c1.896,1.5,4.204,2.249,6.921,2.249c4.31,0,8.28-1.246,11.911-3.737c3.631-2.492,5.516-7.035,5.656-13.633V73.219z",
    b: "M65.102,43.53c5.643,7.152,8.463,16.379,8.463,27.682c0,11.719-2.783,21.431-8.348,29.135c-5.564,7.706-13.333,11.557-23.303,11.557c-6.261,0-11.292-1.246-15.094-3.737c-2.273-1.476-4.731-4.06-7.374-7.751v9.481H0V8.028h19.724v36.264c2.506-3.506,5.269-6.182,8.288-8.028c3.575-2.307,8.126-3.46,13.652-3.46C51.646,32.803,59.459,36.379,65.102,43.53z M48.616,89.482c2.837-4.106,4.257-9.504,4.257-16.194c0-5.352-0.699-9.781-2.094-13.288c-2.651-6.644-7.536-9.965-14.653-9.965c-7.21,0-12.165,3.252-14.863,9.758c-1.396,3.46-2.093,7.936-2.093,13.426c0,6.46,1.441,11.812,4.326,16.056c2.884,4.245,7.28,6.367,13.188,6.367C41.801,95.642,45.778,93.589,48.616,89.482z",
    c: "M49.413,61.523c-0.37-2.813-1.318-5.352-2.844-7.612c-2.221-3.045-5.667-4.568-10.337-4.568c-6.661,0-11.216,3.299-13.667,9.896c-1.295,3.498-1.942,8.146-1.942,13.944c0,5.523,0.647,9.964,1.942,13.323c2.358,6.275,6.798,9.412,13.32,9.412c4.625,0,7.909-1.246,9.851-3.737c1.942-2.492,3.122-5.721,3.538-9.689h20.139c-0.462,5.998-2.633,11.673-6.512,17.024c-6.189,8.628-15.356,12.941-27.501,12.941c-12.146,0-21.083-3.599-26.809-10.796C2.863,94.465,0,85.133,0,73.667c0-12.939,3.163-23.009,9.49-30.206s15.055-10.796,26.187-10.796c9.467,0,17.214,2.123,23.241,6.367c6.026,4.246,9.594,11.742,10.703,22.492H49.413z",
    d: "M44.845,35.675c3.552,2.007,6.436,4.787,8.65,8.339V8.028h20v101.87H54.326v-10.45c-2.815,4.476-6.021,7.729-9.62,9.758c-3.599,2.029-8.074,3.045-13.426,3.045c-8.813,0-16.229-3.564-22.25-10.692C3.01,94.431,0,85.285,0,74.119C0,61.247,2.964,51.12,8.893,43.738c5.928-7.381,13.852-11.073,23.772-11.073C37.232,32.665,41.292,33.668,44.845,35.675z M49.689,89.413c2.907-4.152,4.36-9.526,4.36-16.125c0-9.227-2.33-15.824-6.99-19.793c-2.861-2.398-6.183-3.599-9.965-3.599c-5.768,0-10,2.18-12.699,6.54c-2.699,4.36-4.049,9.77-4.049,16.229c0,6.967,1.373,12.538,4.118,16.713c2.745,4.176,6.909,6.263,12.492,6.263C42.538,95.642,46.783,93.565,49.689,89.413z",
    e: "M55.106,35.992c5.262,2.357,9.607,6.077,13.035,11.159c3.09,4.482,5.094,9.68,6.01,15.595c0.531,3.465,0.748,8.456,0.65,14.971H19.861c0.307,7.567,2.937,12.873,7.889,15.917c3.013,1.892,6.637,2.837,10.875,2.837c4.493,0,8.144-1.153,10.951-3.46c1.531-1.246,2.886-2.976,4.06-5.19h20.137c-0.531,4.476-2.969,9.021-7.312,13.634c-6.758,7.335-16.217,11.003-28.38,11.003c-10.039,0-18.896-3.094-26.57-9.281C3.836,96.99,0,86.923,0,72.977C0,59.91,3.463,49.89,10.392,42.917c6.927-6.972,15.919-10.459,26.975-10.459C43.931,32.458,49.845,33.636,55.106,35.992z M25.601,53.031c-2.788,2.873-4.539,6.765-5.255,11.676h33.98c-0.359-5.236-2.11-9.208-5.255-11.919s-7.044-4.067-11.697-4.067C32.311,48.721,28.387,50.158,25.601,53.031z",
    f: "M38.963,6.955c1.014,0.069,2.398,0.173,4.152,0.312v16.056c-1.107-0.139-2.965-0.242-5.57-0.312c-2.607-0.069-4.406,0.508-5.398,1.73c-0.992,1.223-1.488,2.573-1.488,4.049c0,1.477,0,3.599,0,6.367H43.6v13.91H30.658v60.832H11.074V49.066H0v-13.91h10.797v-4.844c0-8.073,1.359-13.633,4.082-16.679c2.861-4.521,9.76-6.782,20.693-6.782C36.818,6.852,37.947,6.886,38.963,6.955z",
    g: "M41.953,34.395c4.791,1.984,8.662,5.629,11.611,10.935V34.464h19.238v71.558c0,9.734-1.637,17.07-4.912,22.007c-5.631,8.488-16.426,12.734-32.389,12.734c-9.643,0-17.51-1.893-23.6-5.675c-6.09-3.784-9.457-9.436-10.104-16.956h21.453c0.555,2.307,1.453,3.967,2.699,4.983c2.121,1.799,5.697,2.699,10.727,2.699c7.105,0,11.857-2.376,14.258-7.128c1.566-3.045,2.352-8.166,2.352-15.364v-4.844c-1.893,3.23-3.922,5.652-6.09,7.267c-3.922,3-9.02,4.499-15.295,4.499c-9.688,0-17.428-3.402-23.217-10.208C2.895,93.231,0,84.015,0,72.389c0-11.211,2.787-20.634,8.363-28.271c5.574-7.635,13.477-11.453,23.707-11.453C35.848,32.665,39.143,33.242,41.953,34.395z M48.67,88.479c3.172-3.483,4.756-9.031,4.756-16.644c0-7.151-1.506-12.595-4.514-16.333c-3.01-3.737-7.041-5.605-12.094-5.605c-6.893,0-11.646,3.253-14.266,9.758c-1.379,3.46-2.068,7.729-2.068,12.803c0,4.384,0.734,8.282,2.205,11.696c2.664,6.367,7.443,9.55,14.336,9.55C41.617,93.704,45.5,91.963,48.67,88.479z",
    h: "M54.074,34.882c3.789,1.616,6.902,4.087,9.336,7.411c2.066,2.818,3.33,5.716,3.789,8.694c0.461,2.978,0.691,7.839,0.691,14.58v44.332H47.752V63.963c0-4.064-0.689-7.344-2.062-9.838c-1.785-3.51-5.174-5.266-10.166-5.266c-5.176,0-9.104,1.744-11.783,5.231c-2.678,3.487-4.018,8.464-4.018,14.93v40.877H0V8.235h19.723v35.987c2.848-4.383,6.145-7.439,9.889-9.169c3.742-1.73,7.682-2.595,11.816-2.595C46.068,32.458,50.283,33.266,54.074,34.882z",
    i: "M20,25.468H0V7.267h20V25.468z M0,34.464h20v75.434H0V34.464z",
    j: "M0,124.362c1.107,0.092,1.891,0.149,2.354,0.173c0.461,0.023,0.875,0.035,1.246,0.035c1.938,0,3.4-0.474,4.395-1.418c0.99-0.946,1.486-2.78,1.486-5.502V34.81h19.725v83.185c0,7.612-1.5,13.241-4.498,16.886c-3,3.644-8.883,5.467-17.648,5.467c-0.6,0-1.486-0.024-2.664-0.069c-1.176-0.046-2.641-0.093-4.395-0.138V124.362z M29.205,25.468H9.48V7.267h19.725V25.468z",
    k: "M0,8.235h19.377v55.029L44.254,34.81h24.527L41.744,62.912l28.084,46.986H45.824L27.547,77.669l-8.17,8.492v23.737H0V8.235z",
    l: "M19.723,109.898H0V7.89h19.723V109.898z",
    m: "M94.465,34.741c3.229,1.292,6.158,3.553,8.789,6.782c2.121,2.63,3.553,5.86,4.291,9.689c0.461,2.538,0.691,6.252,0.691,11.142l-0.139,47.544H87.891V61.87c0-2.86-0.463-5.213-1.385-7.059c-1.754-3.506-4.982-5.26-9.689-5.26c-5.443,0-9.203,2.261-11.279,6.782c-1.062,2.399-1.592,5.284-1.592,8.651v44.914H44.084V64.984c0-4.475-0.463-7.728-1.385-9.758c-1.662-3.644-4.914-5.467-9.758-5.467c-5.629,0-9.412,1.823-11.35,5.467C20.529,57.302,20,60.394,20,64.5v45.398H0V34.603h19.17v11.004c2.443-3.921,4.75-6.713,6.92-8.374c3.828-2.952,8.789-4.429,14.879-4.429c5.768,0,10.426,1.27,13.98,3.807c2.859,2.353,5.027,5.375,6.504,9.065c2.584-4.429,5.789-7.682,9.619-9.758c4.061-2.076,8.582-3.114,13.564-3.114C87.959,32.803,91.234,33.45,94.465,34.741z",
    n: "M60.689,38.79c4.938,4.083,7.408,10.854,7.408,20.312v50.797H47.889V64.015c0-3.967-0.525-7.012-1.578-9.135c-1.926-3.875-5.59-5.813-10.992-5.813c-6.643,0-11.199,2.837-13.672,8.512c-1.283,3-1.924,6.829-1.924,11.488v40.831H0V34.603h19.1v11.004c2.527-3.875,4.916-6.667,7.168-8.374c4.041-3.045,9.164-4.567,15.367-4.567C49.398,32.665,55.75,34.707,60.689,38.79z",
    o: "M67.545,100.67c-6.367,7.859-16.033,11.789-28.996,11.789c-12.967,0-22.631-3.93-28.998-11.789C3.184,92.811,0,83.349,0,72.285c0-10.878,3.184-20.305,9.551-28.28c6.367-7.975,16.031-11.963,28.998-11.963c12.963,0,22.629,3.988,28.996,11.963c6.367,7.975,9.551,17.402,9.551,28.28C77.096,83.349,73.912,92.811,67.545,100.67z M51.766,89.769c3.092-4.101,4.637-9.928,4.637-17.484c0-7.555-1.545-13.372-4.637-17.448s-7.52-6.116-13.287-6.116s-10.207,2.039-13.322,6.116c-3.113,4.077-4.67,9.893-4.67,17.448c0,7.557,1.557,13.384,4.67,17.484c3.115,4.1,7.555,6.149,13.322,6.149S48.674,93.869,51.766,89.769z",
    p: "M63.99,42.561c6.152,6.505,9.229,16.056,9.229,28.651c0,13.287-3.008,23.415-9.02,30.381c-6.014,6.967-13.758,10.45-23.23,10.45c-6.037,0-11.053-1.5-15.045-4.499c-2.182-1.661-4.318-4.083-6.408-7.267v39.309H0V34.464h18.893v11.142c2.137-3.275,4.41-5.859,6.826-7.751c4.41-3.367,9.656-5.052,15.74-5.052C50.326,32.803,57.838,36.056,63.99,42.561z M48.852,56.749c-2.68-4.475-7.025-6.713-13.039-6.713c-7.225,0-12.188,3.391-14.891,10.173c-1.4,3.599-2.098,8.167-2.098,13.703c0,8.767,2.352,14.926,7.055,18.478c2.797,2.076,6.107,3.114,9.93,3.114c5.547,0,9.779-2.122,12.693-6.367c2.912-4.244,4.371-9.896,4.371-16.955C52.873,66.368,51.531,61.224,48.852,56.749z",
    q: "M47.268,37.101c2.26,1.756,4.568,4.643,6.92,8.663V34.603h19.031v105.192H53.703v-39.239c-1.477,3.045-4.002,5.756-7.578,8.132c-3.576,2.375-8.455,3.564-14.637,3.564c-8.719,0-16.148-3.486-22.283-10.458C3.068,94.82,0,85.515,0,73.878c0-12.56,3.08-22.604,9.24-30.13c6.158-7.526,13.76-11.29,22.803-11.29C38.316,32.458,43.393,34.006,47.268,37.101z M51.484,86.632c1.848-3.834,2.773-8.686,2.773-14.556c0-4.62-0.787-8.755-2.357-12.406c-2.82-6.562-7.768-9.842-14.842-9.842c-4.994,0-9.004,1.918-12.031,5.753c-3.027,3.835-4.543,9.657-4.543,17.467c0,5.222,0.693,9.496,2.082,12.822c2.588,6.33,7.396,9.495,14.424,9.495C43.785,95.365,48.617,92.454,51.484,86.632z",
    r: "M42.111,32.7c0.252,0.023,0.818,0.058,1.695,0.104v20.208c-1.246-0.138-2.354-0.23-3.322-0.277c-0.969-0.045-1.754-0.069-2.354-0.069c-7.936,0-13.264,2.584-15.986,7.751c-1.521,2.907-2.283,7.382-2.283,13.426v36.056H0V34.464h18.822v13.149c3.045-5.028,5.697-8.466,7.959-10.312c3.691-3.09,8.488-4.637,14.395-4.637C41.545,32.665,41.855,32.677,42.111,32.7z",
    s: "M20.002,85.814c0.416,3.507,1.32,5.998,2.711,7.474c2.457,2.63,7,3.945,13.629,3.945c3.895,0,6.988-0.576,9.283-1.73c2.295-1.153,3.441-2.883,3.441-5.19c0-2.215-0.924-3.898-2.771-5.052c-1.85-1.152-8.729-3.137-20.635-5.952c-8.57-2.122-14.609-4.775-18.115-7.958c-3.508-3.137-5.26-7.658-5.26-13.564c0-6.966,2.738-12.952,8.217-17.959c5.48-5.005,13.189-7.509,23.131-7.509c9.432,0,17.117,1.88,23.059,5.64c5.941,3.761,9.352,10.254,10.23,19.481H47.199c-0.279-2.537-0.996-4.544-2.15-6.021c-2.176-2.675-5.877-4.014-11.105-4.014c-4.303,0-7.367,0.669-9.193,2.007c-1.828,1.339-2.742,2.907-2.742,4.706c0,2.261,0.971,3.899,2.916,4.914c1.941,1.062,8.807,2.884,20.592,5.467c7.857,1.846,13.748,4.637,17.67,8.374c3.875,3.784,5.812,8.512,5.812,14.187c0,7.474-2.785,13.576-8.355,18.305c-5.572,4.729-14.182,7.093-25.83,7.093c-11.883,0-20.654-2.503-26.316-7.509C2.832,99.944,0,93.565,0,85.814H20.002z",
    t: "M0,49.205V35.156h10.52V14.118h19.516v21.038h12.248v14.049H30.035v39.862c0,3.091,0.391,5.017,1.176,5.778s3.184,1.142,7.197,1.142c0.6,0,1.234-0.011,1.904-0.035c0.668-0.023,1.324-0.057,1.971-0.104v14.741l-9.342,0.346c-9.32,0.322-15.688-1.291-19.102-4.844c-2.213-2.26-3.32-5.744-3.32-10.45V49.205H0z",
    u: "M20.207,34.464v45.468c0,4.291,0.506,7.521,1.516,9.688c1.791,3.83,5.301,5.744,10.535,5.744c6.703,0,11.293-2.722,13.773-8.166c1.285-2.952,1.928-6.852,1.928-11.696V34.464h20v75.434h-19.17V99.24c-0.184,0.231-0.645,0.923-1.379,2.076c-0.736,1.154-1.611,2.169-2.623,3.045c-3.082,2.769-6.062,4.661-8.938,5.675c-2.875,1.015-6.246,1.523-10.109,1.523c-11.135,0-18.633-4.014-22.498-12.042C1.08,95.088,0,88.56,0,79.932V34.464H20.207z",
    v: "M53.912,34.464h21.176l-27.24,75.434H27.053L0,34.464h22.146l15.709,55.641L53.912,34.464z",
    w: "M43.807,34.464h20l11.488,54.326l11.766-54.326h20.553l-21.869,75.434H65.469L53.633,54.949l-11.971,54.949H21.176L0,34.464h21.176l11.766,54.118L43.807,34.464z",
    x: "M0,109.898l25.744-38.201L1.107,34.603H25.26l12.602,21.85l12.312-21.85h23.461L48.859,71.351l25.744,38.547H50.035L37.053,87.219l-13.107,22.679H0z",
    y: "M10.762,124.362l2.492,0.139c1.938,0.092,3.781,0.022,5.535-0.208c1.754-0.231,3.229-0.761,4.43-1.592c1.152-0.785,2.225-2.422,3.219-4.914c0.99-2.491,1.418-4.014,1.281-4.567L0,34.464h21.938l16.471,55.641L53.98,34.464h20.969l-25.861,74.188c-4.998,14.302-8.951,23.172-11.861,26.609c-2.908,3.437-8.729,5.156-17.459,5.156c-1.754,0-3.164-0.012-4.225-0.035c-1.064-0.024-2.658-0.104-4.781-0.242V124.362z",
    z: "M2.629,50.52V34.464h59.654v16.401L25.27,93.704H63.6v16.194H0V94.534L37.391,50.52H2.629z",
    A: "M36.414,7.89h24.119l36.09,102.008H73.508l-6.73-20.969H29.213l-6.916,20.969H0L36.414,7.89z M35.01,71.351h26.125L48.248,31.211L35.01,71.351z",
    B: "M75.734,18.755c3.121,4.337,4.682,9.527,4.682,15.571c0,6.229-1.574,11.235-4.725,15.018c-1.76,2.123-4.354,4.061-7.779,5.813c5.205,1.892,9.133,4.891,11.783,8.997c2.648,4.107,3.975,9.09,3.975,14.948c0,6.045-1.516,11.465-4.543,16.263c-1.926,3.184-4.334,5.86-7.225,8.028c-3.256,2.492-7.098,4.199-11.525,5.121c-4.426,0.924-9.23,1.384-14.414,1.384H0V7.89h49.297C61.734,8.074,70.549,11.696,75.734,18.755z M20.346,25.606v22.492h24.793c4.43,0,8.025-0.841,10.787-2.526c2.762-1.684,4.145-4.671,4.145-8.962c0-4.751-1.826-7.89-5.479-9.412c-3.152-1.061-7.17-1.592-12.055-1.592H20.346z M20.346,64.984v27.198h24.766c4.424,0,7.867-0.599,10.33-1.8c4.469-2.214,6.705-6.458,6.705-12.733c0-5.305-2.168-8.95-6.5-10.935c-2.416-1.107-5.814-1.684-10.193-1.73H20.346z",
    C: "M14.172,18.547C22.389,10.15,32.84,5.952,45.529,5.952c16.98,0,29.395,5.629,37.246,16.886c4.336,6.321,6.662,12.665,6.982,19.031H68.443c-1.355-4.89-3.098-8.582-5.221-11.073c-3.797-4.429-9.424-6.644-16.881-6.644c-7.594,0-13.584,3.126-17.967,9.377c-4.385,6.252-6.576,15.099-6.576,26.541c0,11.442,2.312,20.012,6.939,25.709c4.625,5.699,10.504,8.547,17.635,8.547c7.311,0,12.885-2.445,16.721-7.335c2.121-2.63,3.881-6.575,5.279-11.834h21.178c-1.826,11.12-6.494,20.163-14.002,27.128c-7.508,6.967-17.127,10.45-28.857,10.45c-14.514,0-25.926-4.706-34.232-14.118C4.152,89.16,0,76.195,0,59.724C0,41.916,4.723,28.19,14.172,18.547z",
    D: "M59.771,10.104c7.156,2.353,12.951,6.667,17.385,12.941c3.555,5.076,5.979,10.566,7.271,16.471c1.293,5.906,1.939,11.535,1.939,16.886c0,13.564-2.725,25.052-8.172,34.464c-7.389,12.688-18.793,19.031-34.215,19.031H0V7.89h43.98C50.307,7.982,55.568,8.72,59.771,10.104z M20.691,25.606v66.576h19.686c10.074,0,17.098-4.959,21.072-14.879c2.17-5.443,3.258-11.926,3.258-19.447c0-10.381-1.631-18.35-4.887-23.91c-3.258-5.559-9.738-8.339-19.443-8.339H20.691z",
    E: "M74.811,25.952H20.832v21.661h49.551V65.33H20.832v26.229h56.471v18.339H0V7.89h74.811V25.952z",
    F: "M0,8.028h72.734v17.924H21.176v23.46h45.191v17.716H21.176v42.769H0V8.028z",
    G: "M73.496,39.724c-1.615-6.966-5.561-11.834-11.834-14.602c-3.508-1.522-7.404-2.284-11.695-2.284c-8.213,0-14.961,3.097-20.242,9.291c-5.285,6.194-7.926,15.506-7.926,27.938c0,12.526,2.861,21.39,8.582,26.593c5.721,5.203,12.227,7.805,19.516,7.805c7.152,0,13.012-2.051,17.578-6.155c4.568-4.104,7.381-9.48,8.443-16.129h-23.6V55.157h42.492v54.741H80.693l-2.145-12.734c-4.107,4.823-7.799,8.223-11.074,10.197c-5.629,3.444-12.549,5.167-20.762,5.167c-13.518,0-24.592-4.68-33.217-14.04C4.498,89.083,0,76.219,0,59.896c0-16.505,4.543-29.739,13.633-39.698S34.74,5.26,49.689,5.26c12.965,0,23.379,3.287,31.246,9.862c7.865,6.574,12.375,14.775,13.529,24.602H73.496z",
    H: "M0,109.898V7.89h21.178v38.893h39.654V7.89H82.01v102.008H60.832V64.361H21.178v45.537H0z",
    I: "M21.178,109.898H0V7.89h21.178V109.898z",
    J: "M20.139,71.212v2.353c0.174,7.889,0.986,13.438,2.438,16.644c1.451,3.208,4.561,4.81,9.328,4.81c4.725,0,7.846-1.753,9.361-5.26c0.91-2.076,1.365-5.582,1.365-10.519V7.89h21.314v71.004c0,8.674-1.465,15.548-4.398,20.623c-4.961,8.582-13.916,12.872-26.863,12.872s-21.633-3.471-26.053-10.415C2.211,95.031,0,85.562,0,73.565v-2.353H20.139z",
    K: "M0,7.89h20.969v42.03L60.381,7.89h27.543L46.064,49.843l44.006,60.055H62.664L31.234,65.37L20.969,75.78v34.118H0V7.89z",
    L: "M0,7.89h21.316v83.669H72.25v18.339H0V7.89z",
    M: "M68.016,7.89h30.67v102.008H78.824V40.9c0-1.983,0.023-4.763,0.068-8.339c0.047-3.575,0.07-6.332,0.07-8.27l-19.332,85.607H38.916L19.723,24.291c0,1.938,0.023,4.695,0.07,8.27c0.045,3.576,0.068,6.356,0.068,8.339v68.998H0V7.89h31.016l18.568,80.208L68.016,7.89z",
    N: "M0,7.89h22.346l40.492,71.127V7.89h19.861v102.008H61.389L19.861,37.519v72.379H0V7.89z",
    O: "M82.355,100.832c-7.705,7.936-18.848,11.903-33.426,11.903c-14.58,0-25.723-3.967-33.428-11.903C5.168,91.098,0,77.072,0,58.755c0-18.686,5.168-32.71,15.502-42.077C23.207,8.744,34.35,4.775,48.93,4.775c14.578,0,25.721,3.968,33.426,11.903c10.287,9.366,15.432,23.391,15.432,42.077C97.787,77.072,92.643,91.098,82.355,100.832z M69.033,85.33c4.959-6.229,7.439-15.087,7.439-26.575c0-11.441-2.48-20.288-7.439-26.54c-4.961-6.251-11.66-9.377-20.104-9.377s-15.18,3.114-20.209,9.343s-7.543,15.086-7.543,26.575c0,11.488,2.514,20.346,7.543,26.575s11.766,9.343,20.209,9.343S64.072,91.559,69.033,85.33z",
    P: "M68,65.745c-5.973,4.983-14.5,7.474-25.584,7.474H21.178v36.679H0V7.89h43.801c10.098,0,18.146,2.629,24.15,7.889s9.006,13.403,9.006,24.43C76.957,52.25,73.971,60.762,68,65.745z M51.73,28.997c-2.699-2.26-6.48-3.391-11.344-3.391H21.178v30.035h19.209c4.863,0,8.645-1.222,11.344-3.668c2.699-2.445,4.049-6.32,4.049-11.626C55.779,35.042,54.43,31.258,51.73,28.997z",
    Q: "M94.465,80.07c-1.793,5.85-4.438,10.708-7.932,14.577l11.711,11.029l-11.115,11.599l-12.252-11.626c-3.74,2.27-6.973,3.868-9.697,4.794c-4.57,1.528-10.043,2.292-16.416,2.292c-13.299,0-24.289-3.967-32.971-11.903C5.264,91.282,0,77.257,0,58.755c0-18.639,5.398-32.734,16.193-42.285c8.812-7.796,19.77-11.695,32.873-11.695c13.195,0,24.268,4.129,33.219,12.388c10.334,9.55,15.502,22.907,15.502,40.07C97.787,66.322,96.68,73.935,94.465,80.07z M55.791,93.771c1.246-0.323,2.838-0.901,4.777-1.733l-10.326-9.822l10.951-11.433l10.367,9.764c1.613-3.321,2.744-6.228,3.391-8.719c1.014-3.736,1.521-8.095,1.521-13.078c0-11.44-2.344-20.286-7.025-26.536c-4.684-6.25-11.523-9.376-20.52-9.376c-8.443,0-15.18,3-20.207,8.997c-5.029,5.998-7.543,14.972-7.543,26.921c0,13.979,3.6,23.991,10.799,30.035c4.66,3.922,10.246,5.882,16.754,5.882C51.176,94.673,53.529,94.372,55.791,93.771z",
    R: "M64.887,10.519c3.756,1.616,6.939,3.991,9.549,7.128c2.158,2.584,3.869,5.444,5.129,8.582c1.26,3.138,1.891,6.713,1.891,10.727c0,4.844-1.225,9.609-3.668,14.291c-2.447,4.683-6.482,7.993-12.111,9.931c4.705,1.893,8.039,4.58,10,8.062c1.961,3.484,2.941,8.801,2.941,15.952v6.851c0,4.661,0.188,7.82,0.562,9.481c0.562,2.63,1.873,4.568,3.936,5.813v2.561H59.586c-0.646-2.26-1.107-4.083-1.385-5.467c-0.553-2.86-0.854-5.79-0.9-8.789l-0.137-9.481c-0.088-6.505-1.213-10.841-3.373-13.011c-2.16-2.168-6.209-3.252-12.143-3.252H20.83v40H0V7.89h48.791C55.764,8.028,61.129,8.905,64.887,10.519z M20.83,25.606v27.405H43.77c4.557,0,7.975-0.554,10.254-1.661c4.031-1.938,6.047-5.767,6.047-11.488c0-6.182-1.951-10.334-5.85-12.457c-2.191-1.199-5.479-1.799-9.859-1.799H20.83z",
    S: "M20.346,78.41c0.66,4.661,1.955,8.144,3.887,10.45c3.533,4.199,9.586,6.298,18.16,6.298c5.135,0,9.305-0.554,12.508-1.661c6.076-2.122,9.115-6.066,9.115-11.834c0-3.367-1.49-5.974-4.465-7.82c-2.979-1.799-7.699-3.391-14.164-4.775l-11.043-2.422c-10.855-2.398-18.312-5.005-22.371-7.82C5.098,54.119,1.662,46.76,1.662,36.748c0-9.135,3.361-16.724,10.086-22.769c6.727-6.043,16.605-9.066,29.637-9.066c10.881,0,20.162,2.85,27.846,8.547c7.684,5.699,11.711,13.969,12.086,24.81H60.832c-0.379-6.135-3.123-10.496-8.23-13.08c-3.406-1.707-7.639-2.561-12.699-2.561c-5.629,0-10.123,1.107-13.48,3.322c-3.359,2.214-5.037,5.306-5.037,9.273c0,3.645,1.656,6.367,4.967,8.166c2.127,1.2,6.645,2.607,13.553,4.222l17.9,4.222c7.846,1.846,13.727,4.314,17.643,7.405c6.082,4.799,9.121,11.742,9.121,20.831c0,9.32-3.602,17.059-10.807,23.219c-7.205,6.159-17.383,9.239-30.533,9.239c-13.43,0-23.992-3.033-31.688-9.101C3.846,97.361,0,89.022,0,78.41H20.346z",
    T: "M82.631,7.89v18.062h-30.52v83.946H30.656V25.952H0V7.89H82.631z",
    U: "M0,7.89h21.662v62.65c0,7.007,0.826,12.124,2.482,15.351c2.574,5.717,8.182,8.575,16.826,8.575c8.596,0,14.182-2.858,16.756-8.575c1.654-3.227,2.482-8.343,2.482-15.351V7.89h21.662v62.7c0,10.843-1.682,19.286-5.045,25.329c-6.264,11.073-18.215,16.609-35.855,16.609s-29.617-5.537-35.928-16.609C1.682,89.875,0,81.432,0,70.589V7.89z",
    V: "M67.268,7.89h22.146L54.637,109.898H34.498L0,7.89h22.77l22.145,77.44L67.268,7.89z",
    W: "M22.631,7.89l13.467,58.416l2.934,16.257l2.943-15.924L53.496,7.89h22.492l12.119,58.407l3.105,16.266l3.148-15.633l13.6-59.04h21.701l-28.697,102.008h-20.41L68.236,50.243L64.639,30.52l-3.6,19.724l-12.318,59.655H28.924L0,7.89H22.631z",
    X: "M24.775,109.898H0l32.805-51.973L1.592,7.89h25.469L45.254,41.11L63.945,7.89h24.639L57.371,57.094l33.15,52.804H64.639L45.256,75.116L24.775,109.898z",
    Y: "M66.576,7.89h24.152L56.609,71.649v38.249H35.295V71.649L0,7.89h25.121l21.178,44.43L66.576,7.89z",
    Z: "M0,91.905l52.139-65.953H1.314V7.89h77.303v17.093L25.803,91.905h52.953v17.993H0V91.905z",
    0: "M61.49,22.354c5.143,8.997,7.717,21.777,7.717,38.339c0,16.564-2.574,29.32-7.717,38.271c-5.145,8.951-14.107,13.426-26.887,13.426s-21.742-4.475-26.887-13.426C2.572,90.013,0,77.257,0,60.693C0,44.13,2.572,31.35,7.717,22.354c5.145-8.997,14.107-13.495,26.887-13.495S56.346,13.356,61.49,22.354z M22.977,87.164c1.893,6.114,5.768,9.169,11.627,9.169s9.699-3.056,11.523-9.169c1.82-6.113,2.732-14.937,2.732-26.471c0-12.087-0.912-21.038-2.732-26.852c-1.824-5.813-5.664-8.72-11.523-8.72s-9.734,2.907-11.627,8.72c-1.893,5.813-2.838,14.765-2.838,26.852C20.139,72.228,21.084,81.051,22.977,87.164z",
    1: "M0,40.693V27.267c6.211-0.277,10.561-0.692,13.045-1.246c3.959-0.875,7.182-2.629,9.666-5.259c1.701-1.799,2.99-4.198,3.865-7.197c0.506-1.799,0.76-3.137,0.76-4.014h16.471v100.348H23.598V40.693H0z",
    2: "M4.637,90.174c2.814-6.689,9.459-13.771,19.932-21.246c9.09-6.505,14.971-11.165,17.648-13.979c4.105-4.383,6.158-9.181,6.158-14.395c0-4.244-1.176-7.773-3.529-10.588c-2.354-2.813-5.721-4.221-10.104-4.221c-5.998,0-10.082,2.238-12.25,6.713c-1.246,2.584-1.984,6.69-2.215,12.318H1.107C1.43,36.241,2.979,29.343,5.75,24.083C11.014,14.073,20.367,9.066,33.807,9.066c10.621,0,19.074,2.941,25.355,8.824c6.279,5.882,9.422,13.668,9.422,23.357c0,7.429-2.219,14.026-6.658,19.792c-2.912,3.83-7.695,8.097-14.352,12.803l-7.904,5.606c-4.947,3.507-8.33,6.044-10.152,7.612c-1.824,1.569-3.357,3.391-4.604,5.467h43.809v17.371H0C0.186,102.701,1.73,96.126,4.637,90.174z",
    3: "M19.377,79.102c0,4.014,0.645,7.336,1.934,9.966c2.395,4.844,6.742,7.266,13.049,7.266c3.867,0,7.238-1.326,10.115-3.979c2.877-2.652,4.314-6.471,4.314-11.454c0-6.597-2.666-11.003-8.002-13.218c-3.037-1.246-7.818-1.868-14.35-1.868V51.696c6.391-0.092,10.85-0.715,13.379-1.869c4.367-1.938,6.551-5.858,6.551-11.765c0-3.829-1.115-6.943-3.348-9.343c-2.23-2.398-5.373-3.599-9.422-3.599c-4.646,0-8.064,1.477-10.25,4.429c-2.186,2.953-3.232,6.898-3.139,11.834H1.799c0.186-4.983,1.037-9.711,2.559-14.187c1.613-3.921,4.148-7.543,7.605-10.865c2.58-2.353,5.645-4.152,9.193-5.398s7.904-1.869,13.066-1.869c9.586,0,17.316,2.48,23.193,7.439c5.875,4.96,8.814,11.616,8.814,19.966c0,5.906-1.754,10.889-5.26,14.948c-2.215,2.538-4.523,4.268-6.922,5.19c1.801,0,4.383,1.546,7.752,4.637c5.027,4.661,7.543,11.027,7.543,19.101c0,8.49-2.939,15.952-8.814,22.388c-5.877,6.436-14.576,9.654-26.1,9.654c-14.195,0-24.059-4.637-29.59-13.91C1.936,93.543,0.322,87.084,0,79.102H19.377z",
    4: "M70.383,88.306h-11.35v21.592H39.725V88.306H0V71.074l36.887-60.9h22.146v62.7h11.35V88.306z M39.725,72.873V29.651L14.678,72.873H39.725z",
    5: "M19.654,83.046c0.781,4.291,2.277,7.602,4.486,9.931c2.209,2.331,5.432,3.495,9.666,3.495c4.879,0,8.596-1.718,11.15-5.156c2.555-3.437,3.832-7.762,3.832-12.976c0-5.121-1.197-9.446-3.59-12.976c-2.395-3.529-6.123-5.294-11.186-5.294c-2.395,0-4.465,0.3-6.213,0.899c-3.084,1.107-5.41,3.161-6.975,6.159l-17.68-0.83l7.041-55.434h55.211v16.748h-40.98l-3.592,21.938c3.041-1.983,5.414-3.298,7.119-3.944c2.857-1.061,6.336-1.592,10.436-1.592c8.293,0,15.525,2.792,21.701,8.374c6.174,5.583,9.262,13.703,9.262,24.36c0,9.273-2.975,17.556-8.92,24.845c-5.945,7.291-14.842,10.935-26.686,10.935c-9.543,0-17.377-2.561-23.508-7.682C4.102,99.725,0.689,92.458,0,83.046H19.654z",
    6: "M47.545,34.81c0-1.614-0.623-3.391-1.869-5.329c-2.123-3.137-5.328-4.706-9.619-4.706c-6.414,0-10.98,3.599-13.703,10.796c-1.477,3.968-2.49,9.827-3.045,17.578c2.445-2.907,5.283-5.028,8.514-6.367c3.229-1.337,6.92-2.007,11.072-2.007c8.904,0,16.205,3.022,21.902,9.066c5.699,6.044,8.547,13.772,8.547,23.184c0,9.367-2.791,17.625-8.373,24.775c-5.582,7.152-14.256,10.727-26.021,10.727c-12.641,0-21.961-5.282-27.959-15.848C2.33,88.421,0,77.764,0,64.707c0-7.658,0.322-13.887,0.971-18.686c1.152-8.535,3.391-15.64,6.713-21.315c2.859-4.845,6.607-8.743,11.244-11.696c4.637-2.952,10.186-4.429,16.645-4.429c9.318,0,16.748,2.388,22.285,7.163c5.535,4.775,8.65,11.131,9.342,19.066H47.545z M24.154,91.351c2.906,3.415,6.596,5.121,11.072,5.121c4.383,0,7.83-1.649,10.346-4.948c2.514-3.298,3.771-7.578,3.771-12.837c0-5.859-1.43-10.346-4.291-13.46s-6.365-4.671-10.518-4.671c-3.369,0-6.346,1.015-8.928,3.045c-3.877,3-5.814,7.844-5.814,14.533C19.793,83.531,21.246,87.937,24.154,91.351z",
    7: "M71.143,26.298c-2.957,2.907-7.072,8.086-12.342,15.537c-5.27,7.452-9.684,15.146-13.242,23.08c-2.82,6.229-5.363,13.841-7.627,22.838c-2.266,8.997-3.398,16.379-3.398,22.146H14.049c0.6-17.993,6.512-36.702,17.73-56.125c7.25-12.042,13.322-20.438,18.217-25.19H0l0.277-17.717h70.865V26.298z",
    8: "M3.816,66.783c2.545-4.568,6.27-8.004,11.176-10.312c-4.863-3.229-8.023-6.724-9.482-10.485c-1.459-3.76-2.188-7.277-2.188-10.554c0-7.289,2.75-13.506,8.252-18.651c5.502-5.144,13.271-7.716,23.305-7.716c10.035,0,17.803,2.573,23.305,7.716c5.502,5.145,8.254,11.362,8.254,18.651c0,3.276-0.727,6.794-2.18,10.554c-1.453,3.761-4.604,7.024-9.447,9.792c4.959,2.769,8.691,6.436,11.193,11.004c2.504,4.567,3.756,9.666,3.756,15.294c0,8.443-3.131,15.629-9.387,21.557c-6.258,5.929-15,8.893-26.221,8.893c-11.223,0-19.721-2.964-25.492-8.893C2.887,97.707,0,90.521,0,82.078C0,76.449,1.271,71.351,3.816,66.783z M24.326,92.25c2.561,2.723,6.102,4.083,10.623,4.083s8.062-1.36,10.623-4.083c2.561-2.722,3.84-6.551,3.84-11.488c0-5.121-1.303-9.008-3.91-11.661c-2.605-2.653-6.123-3.979-10.553-3.979s-7.947,1.327-10.555,3.979c-2.607,2.653-3.91,6.54-3.91,11.661C20.484,85.7,21.766,89.529,24.326,92.25z M25.58,46.575c2.244,2.308,5.355,3.46,9.334,3.46c4.025,0,7.137-1.153,9.334-3.46c2.197-2.307,3.297-5.282,3.297-8.927c0-3.968-1.1-7.07-3.297-9.309c-2.197-2.237-5.309-3.356-9.334-3.356c-3.979,0-7.09,1.12-9.334,3.356c-2.244,2.239-3.365,5.341-3.365,9.309C22.215,41.293,23.336,44.269,25.58,46.575z",
    9: "M9.307,18.72c6.205-6.667,14.268-10,24.188-10c15.271,0,25.744,6.76,31.42,20.277c3.229,7.659,4.844,17.74,4.844,30.243c0,12.135-1.547,22.261-4.637,30.381c-5.906,15.457-16.748,23.184-32.525,23.184c-7.521,0-14.281-2.227-20.277-6.678C6.32,101.674,2.883,95.181,2.006,86.645H21.66c0.461,2.953,1.707,5.353,3.738,7.197c2.029,1.846,4.729,2.768,8.096,2.768c6.506,0,11.074-3.599,13.703-10.796c1.43-3.967,2.33-9.758,2.699-17.371c-1.799,2.261-3.715,3.991-5.744,5.19c-3.691,2.215-8.236,3.322-13.633,3.322c-7.982,0-15.064-2.756-21.246-8.27C3.09,63.174,0,55.226,0,44.845C0,34.096,3.102,25.388,9.307,18.72z M42.492,58.548c4.475-2.86,6.713-7.82,6.713-14.879c0-5.675-1.328-10.173-3.98-13.495c-2.652-3.322-6.287-4.982-10.9-4.982c-3.367,0-6.252,0.946-8.65,2.837C21.891,30.981,20,35.987,20,43.046c0,5.952,1.211,10.415,3.633,13.391c2.422,2.976,6.148,4.464,11.178,4.464C37.531,60.901,40.092,60.117,42.492,58.548z",
    "-": "M0,61.039h38.617v18.686H0V61.039z",
    "=": "M74.949,47.129V65.33H0V47.129H74.949z M74.949,79.448v18.201H0V79.448H74.949z",
    "_": "M0,127.614v-6.989h79.102v6.989H0z",
    "+": "M0,81.385V63.323h28.375V34.949h18.201v28.374h28.373v18.062H46.576v28.513H28.375V81.385H0z",
    "[": "M0,6.92h34.742v14.395H18.686v101.385h16.057v14.464H0V6.92z",
    "]": "M0,122.701h16.055V21.039H0V6.92h34.74v130.245H0V122.701z",
    "{": "M14.395,94.327c0-5.813-1.754-10.265-5.26-13.356C7.012,79.079,3.967,77.579,0,76.472v-8.374c4.475-1.522,7.635-3.206,9.48-5.052c3.275-3.183,4.914-8.05,4.914-14.602V29.896c0-2.353,0.391-4.82,1.174-7.405c1.381-4.475,3.727-8.028,7.041-10.658c3.039-2.398,6.605-3.99,10.703-4.775c2.484-0.507,6.053-0.784,10.701-0.831v11.073c-3.807,0.831-6.396,1.936-7.771,3.317c-2.107,2.12-3.162,5.83-3.162,11.128v16.036c0,5.161-0.693,9.261-2.076,12.302c-2.354,5.116-6.967,9.263-13.842,12.442c6.229,2.535,10.416,5.484,12.561,8.847c2.146,3.364,3.264,8.018,3.357,13.961v17.487c0,5.115,1.236,8.801,3.713,11.059c1.375,1.336,3.783,2.511,7.221,3.525v11.073l-4.9-0.138c-6.584-0.185-12.35-2.157-17.297-5.917c-4.947-3.761-7.422-9.239-7.422-16.437V94.327z",
    "}": "M0,138.479v-11.073c3.461-1.014,5.883-2.212,7.268-3.594c2.352-2.304,3.574-5.968,3.668-10.99V95.335c0-6.542,1.271-11.392,3.82-14.548c2.547-3.157,6.598-5.91,12.154-8.26c-6.066-2.949-10.246-6.301-12.537-10.057c-2.293-3.755-3.438-8.652-3.438-14.688V31.747c0-5.115-0.82-8.594-2.457-10.436C6.84,19.468,4.014,18.132,0,17.301V6.229c4.625,0.046,8.186,0.323,10.684,0.831c4.115,0.785,7.721,2.376,10.82,4.775c3.098,2.399,5.248,5.353,6.451,8.858c1.203,3.507,1.803,6.575,1.803,9.204v18.547c0,6.598,1.5,11.442,4.5,14.533c1.844,1.846,5.143,3.553,9.896,5.121v8.374c-3.924,1.107-6.898,2.538-8.928,4.291c-3.646,3.138-5.469,7.659-5.469,13.564v21.661c0,7.013-2.428,12.445-7.283,16.298c-4.855,3.852-10.705,5.871-17.549,6.056L0,138.479z",
    "\\": "M16.254,4.775l39.689,105.123H39.539L0,4.775H16.254z",
    "|": "M18.201,6.92v102.978H0V6.92H18.201z",
    ";": "M0,37.371h20.969v20.692H0V37.371z M0,126.3c4.66-1.339,7.912-3.714,9.758-7.128c1.152-2.123,1.893-5.214,2.215-9.273H0V89.206h21.316v17.909c0,3.193-0.416,6.486-1.246,9.877c-0.83,3.391-2.377,6.332-4.637,8.823c-2.354,2.63-5.191,4.637-8.512,6.021C3.6,133.22,1.291,133.912,0,133.912V126.3z",
    "'": "M18.893,7.89l-3.461,42.146H3.459L0,7.89H18.893z",
    ":": "M0,37.371h20.969v20.692H0V37.371z M0,89.206h20.969v20.692H0V89.206z",
    "\"": "M18.893,7.89l-3.32,42.146H3.115L0,7.89H18.893z M45.123,7.89l-3.254,42.146H29.412L26.229,7.89H45.123z",
    ",": "M0,126.3c3.967-1.107,6.885-2.953,8.754-5.537c1.869-2.584,2.941-6.206,3.219-10.865H0V89.206h21.314v17.91c0,3.146-0.414,6.426-1.246,9.841c-0.83,3.414-2.375,6.367-4.637,8.858c-2.445,2.675-5.316,4.694-8.615,6.056c-3.299,1.36-5.57,2.042-6.816,2.042V126.3z",
    ".": "M0,89.206h20.969v20.692H0V89.206z",
    "/": "M39.564,4.775h16.41L16.264,109.898H0L39.564,4.775z",
    "<": "M0,63.496l80.9-31.004v19.327L23.264,72.353L80.9,92.959v19.257L0,81.212V63.496z",
    ">": "M80.9,81.212L0,112.216V92.959l57.705-20.632L0,51.818V32.492l80.9,31.143V81.212z",
    "?": "M13.771,12.111c5.443-3.505,12.133-5.259,20.068-5.259c10.428,0,19.09,2.491,25.986,7.474c6.898,4.982,10.348,12.365,10.348,22.146c0,5.998-1.496,11.05-4.486,15.156c-1.75,2.492-5.107,5.675-10.078,9.55l-4.898,3.807c-2.67,2.076-4.441,4.498-5.314,7.267c-0.553,1.754-0.854,4.476-0.898,8.166H25.744c0.275-7.796,1.012-13.184,2.207-16.159c1.195-2.976,4.277-6.402,9.246-10.277l5.037-3.944c1.656-1.246,2.99-2.606,4.002-4.083c1.84-2.537,2.76-5.329,2.76-8.374c0-3.506-1.023-6.701-3.072-9.585c-2.047-2.883-5.787-4.325-11.219-4.325c-5.34,0-9.127,1.776-11.357,5.329C21.115,32.55,20,36.241,20,40.07H0C0.553,26.921,5.143,17.602,13.771,12.111z M25.26,89.897h20.691v20H25.26V89.897z",
    "`": "M35.227,24.291H20.969L0,3.599h21.938L35.227,24.291z",
    "~": "M3.375,74.604c1.461-3.968,3.26-7.22,5.395-9.758c2.273-2.675,4.617-4.556,7.029-5.64c2.412-1.083,4.896-1.626,7.447-1.626c1.113,0,2.574,0.139,4.385,0.416c2.877,0.461,5.549,1.154,8.018,2.076l16.273,6.229c1.908,0.738,3.758,1.257,5.551,1.557c1.793,0.3,3.178,0.45,4.156,0.45c3.072,0,5.516-1.188,7.332-3.564c1.814-2.376,3.143-5.248,3.98-8.616h11.766c-1.254,8.582-3.816,15.941-7.691,22.077c-3.875,6.136-9.105,9.204-15.695,9.204c-1.299,0-2.668-0.115-4.105-0.346c-2.785-0.415-6.086-1.407-9.904-2.976l-14.596-5.467c-1.352-0.507-3.004-0.969-4.959-1.384c-1.955-0.415-3.398-0.623-4.33-0.623c-2.654,0-5.006,1.003-7.053,3.01c-2.049,2.007-3.516,5.064-4.4,9.17H0C0.789,83.3,1.912,78.572,3.375,74.604z",
    "!": "M0,8.235h21.66v25.858l-5.605,47.569H5.744L0,34.093V8.235z M0.484,89.897h20.691v20H0.484V89.897z",
    "@": "M33.115,80.14c-3.715-4.014-5.572-9.62-5.572-16.817c0-8.35,2.838-16.17,8.514-23.46c5.674-7.289,12.641-10.935,20.898-10.935c4.846,0,8.605,1.339,11.281,4.014c2.029,2.076,3.391,4.453,4.082,7.128l2.492-8.72h13.08l-8.857,30.035c-0.693,2.446-1.189,4.256-1.488,5.433c-0.301,1.176-0.451,2.204-0.451,3.08c0,1.384,0.461,2.688,1.385,3.91c0.922,1.223,2.398,1.834,4.43,1.834c3.967,0,7.945-2.595,11.938-7.786c3.99-5.19,5.986-12.077,5.986-20.658c0-12.688-5.445-21.8-16.332-27.336c-6.83-3.505-14.512-5.259-23.045-5.259c-15.688,0-28.283,4.799-37.787,14.395c-8.582,8.674-12.873,19.078-12.873,31.211c0,13.473,5.029,24.153,15.088,32.042c8.904,6.967,19.77,10.45,32.596,10.45c8.766,0,16.955-1.615,24.568-4.844c4.152-1.707,8.441-4.129,12.871-7.267l1.662-1.177l5.051,7.751c-6.551,5.076-13.645,8.939-21.279,11.592c-7.637,2.653-15.561,3.979-23.773,3.979c-19.146,0-33.979-5.952-44.498-17.855C4.359,85.008,0,73.312,0,59.793c0-15.132,5.443-27.959,16.332-38.478C27.773,10.197,42.422,4.637,60.277,4.637c14.533,0,26.574,3.806,36.125,11.419c10.059,8.028,15.088,18.686,15.088,31.973c0,10.474-3.184,19.458-9.551,26.956C95.572,82.482,88.307,86.23,80.139,86.23c-4.244,0-7.508-1.199-9.791-3.599c-2.285-2.398-3.426-4.867-3.426-7.405c0-0.322,0.01-0.68,0.033-1.073c0.023-0.391,0.059-0.818,0.105-1.28c-1.754,3.322-3.715,5.976-5.883,7.958c-3.922,3.554-8.629,5.329-14.119,5.329C41.477,86.161,36.828,84.154,33.115,80.14z M66.16,40.9c-1.477-1.661-3.461-2.491-5.951-2.491c-4.799,0-8.928,2.987-12.389,8.962c-3.459,5.976-5.189,11.754-5.189,17.336c0,3.368,0.771,6.09,2.318,8.166c1.545,2.076,3.725,3.114,6.539,3.114c4.938,0,8.984-3.748,12.146-11.246c3.158-7.497,4.74-13.392,4.74-17.682C68.375,44.615,67.637,42.561,66.16,40.9z",
    "#": "M3.094,70.659h14.01l5.479-20.277H8.523l3.094-11.557h14.088l7.555-27.959h13.762l-7.555,27.959h11.326l7.496-27.959h13.758l-7.498,27.959h14.068l-3.096,11.557h-14.07l-5.438,20.277h14.08L67,82.216H52.916l-7.422,27.682H31.738l7.422-27.682H27.742l-7.48,27.682H6.5l7.48-27.682H0L3.094,70.659z M42.258,70.659l5.438-20.277H36.344l-5.479,20.277H42.258z",
    "$": "M38.689,126.3h-6.713v-13.772c-9.09-1.016-15.871-2.999-20.346-5.952C3.742,101.271-0.135,92.228,0.003,79.448h18.686c0.646,5.813,1.545,9.712,2.699,11.696c1.799,3.091,5.328,5.098,10.588,6.021v-29.55l-5.605-1.661c-8.812-2.583-15.029-6.182-18.65-10.796c-3.623-4.613-5.432-10.173-5.432-16.679c0-4.291,0.701-8.189,2.109-11.695c1.408-3.506,3.379-6.528,5.918-9.066c3.275-3.275,6.92-5.537,10.934-6.782c2.445-0.784,6.021-1.36,10.727-1.73V0h6.713v9.343c7.486,0.6,13.363,2.446,17.635,5.537c7.762,4.937,11.779,12.941,12.055,24.014H50.177c-0.365-4.106-1.049-7.104-2.051-8.997c-1.732-3.275-4.877-5.074-9.438-5.398v26.367c10.934,3.784,18.281,7.128,22.049,10.035c6.201,4.844,9.303,11.673,9.303,20.484c0,11.626-4.27,20.07-12.805,25.329c-5.213,3.229-11.395,5.122-18.547,5.675V126.3z M31.976,24.637c-4.014,0.093-6.988,1.166-8.926,3.218c-1.939,2.054-2.908,4.856-2.908,8.409c0,3.875,1.453,6.944,4.361,9.204c1.613,1.246,4.105,2.399,7.473,3.46V24.637z M38.689,96.957c3.604-0.46,6.227-1.291,7.869-2.491c2.875-2.122,4.312-5.79,4.312-11.004c0-3.967-1.348-7.104-4.039-9.412c-1.596-1.338-4.311-2.722-8.143-4.152V96.957z",
    "%": "M43.98,53.98c-5.008,5.029-11.086,7.543-18.236,7.543c-7.105,0-13.172-2.514-18.201-7.543C2.514,48.952,0,42.885,0,35.779c0-7.104,2.514-13.172,7.543-18.201c5.029-5.028,11.096-7.543,18.201-7.543s13.172,2.515,18.201,7.543c5.029,5.029,7.543,11.097,7.543,18.201C51.488,42.885,48.986,48.952,43.98,53.98z M33.529,43.565c2.146-2.146,3.219-4.741,3.219-7.786c0-3.045-1.072-5.64-3.219-7.786c-2.145-2.146-4.74-3.218-7.785-3.218s-5.641,1.073-7.785,3.218s-3.219,4.74-3.219,7.786c0,3.045,1.074,5.64,3.219,7.786c2.145,2.145,4.74,3.218,7.785,3.218S31.385,45.71,33.529,43.565z M82.008,9.55h10.479L36.594,112.528H25.883L82.008,9.55z M111.213,104.361c-5.029,5.029-11.096,7.543-18.201,7.543s-13.172-2.514-18.201-7.543c-5.029-5.028-7.543-11.095-7.543-18.201c0-7.151,2.514-13.229,7.543-18.236c5.029-5.005,11.096-7.509,18.201-7.509s13.172,2.515,18.201,7.543c5.027,5.029,7.543,11.097,7.543,18.201C118.756,93.266,116.24,99.333,111.213,104.361z M100.797,78.375c-2.145-2.146-4.74-3.218-7.785-3.218s-5.641,1.073-7.785,3.218s-3.219,4.741-3.219,7.786s1.074,5.64,3.219,7.786c2.145,2.145,4.74,3.218,7.785,3.218s5.641-1.073,7.785-3.218c2.146-2.146,3.219-4.741,3.219-7.786S102.943,80.521,100.797,78.375z",
    "^": "M41.039,7.89l25.398,61.177H48.98L33.283,29.985l-15.82,39.082H0L25.469,7.89H41.039z",
    "&": "M4.568,67.261c3.045-4.384,8.027-8.721,14.947-13.013l2.146-1.315c-2.953-3.326-5.236-6.722-6.852-10.187c-1.615-3.464-2.422-7.068-2.422-10.811c0-7.438,2.52-13.247,7.564-17.429c5.041-4.181,11.578-6.272,19.609-6.272c7.33,0,13.387,2.141,18.174,6.421c4.785,4.281,7.18,9.754,7.18,16.418c0,6.017-1.436,10.899-4.307,14.646c-2.869,3.749-7.08,7.22-12.633,10.414l14.658,17.774c1.682-2.398,2.979-5.004,3.889-7.818c0.91-2.813,1.389-5.79,1.438-8.926h17.508c-0.322,6.202-1.818,12.472-4.486,18.812c-1.475,3.564-3.865,7.497-7.18,11.8l18.518,22.122H68.445l-7.096-8.641c-3.453,3.201-6.67,5.567-9.654,7.097c-5.316,2.735-11.449,4.104-18.396,4.104c-10.447,0-18.607-3.045-24.484-9.136C2.939,97.232,0,90.472,0,83.042C0,76.905,1.523,71.645,4.568,67.261z M23.846,91.691c2.793,2.818,6.371,4.227,10.738,4.227c3.285,0,6.396-0.741,9.33-2.224c2.934-1.482,5.316-3.136,7.148-4.961L31.98,65.498c-4.996,3.281-8.299,6.168-9.91,8.663s-2.416,5.243-2.416,8.246C19.654,85.779,21.051,88.874,23.846,91.691z M32.531,37.054c0.859,1.341,2.725,3.721,5.598,7.142c2.871-1.941,4.928-3.583,6.172-4.923c2.393-2.496,3.59-5.201,3.59-8.114c0-2.126-0.719-4.021-2.154-5.686c-1.436-1.665-3.613-2.497-6.531-2.497c-1.818,0-3.518,0.486-5.096,1.457c-2.393,1.434-3.59,3.699-3.59,6.796C30.52,33.078,31.189,35.02,32.531,37.054z",
    "*": "M3.258,19.272l14.459,4.667V7.267h11.973v16.677l14.461-4.666l3.256,10.304l-14.74,4.536l9.619,13.772l-8.859,6.298l-9.598-13.016L13.91,54.188L5.053,47.89l9.688-13.772L0,29.582L3.258,19.272z",
    "(": "M8.996,32.458c3.322-7.658,6.896-14.141,10.727-19.447l4.914-6.782l11.35,0.138c-6.412,11.72-10.773,20.969-13.08,27.751c-3.83,11.35-5.744,24.038-5.744,38.063c0,9.043,0.738,17.371,2.215,24.983c2.26,11.673,6.299,22.86,12.111,33.564l4.221,7.751H24.014c-7.473-10.242-13.172-20.323-17.094-30.243C2.307,96.704,0,85.1,0,73.427C0,59.956,2.998,46.298,8.996,32.458z",
    ")": "M31.156,33.634c5.389,12.457,8.084,25.007,8.084,37.647c0,13.888-3.201,28.005-9.602,42.354c-3.594,8.027-6.98,14.071-10.156,18.131l-4.906,6.713H0c6.068-10.98,10.297-19.978,12.688-26.99c4.092-11.857,6.137-24.845,6.137-38.962c0-9.042-0.734-17.394-2.205-25.053c-2.254-11.672-6.275-22.837-12.066-33.495L0.346,6.229h14.854C22.199,16.01,27.518,25.146,31.156,33.634z"
};